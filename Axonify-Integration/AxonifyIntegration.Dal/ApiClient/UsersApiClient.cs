using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;

using AxonifyIntegration.Models.Api;
using AxonifyIntegration.Models.Api.Requests;
using AxonifyIntegration.Models.Api.Results;
using AxonifyIntegration.Models.Constants;
using AxonifyIntegration.Dal.Repositories;

namespace AxonifyIntegration.Dal.ApiClient
{
    public class UsersApiClient
    {
        /// <summary>
        /// Call an Axonify api tu create or update users
        /// </summary>
        /// <returns></returns>
        public GeneralResult SendPendingUsers()
        {
            InterfacesRepository.InterfaceHistoryUpdate(InterfacesActions.NEWRECORD, InterfacesNames.AxonifyUsers);
            Console.WriteLine("----Execution Interface: " + InterfacesNames.AxonifyUsers);
            GeneralResult result = new GeneralResult();

            try
            {
                Console.WriteLine("--------Getting users from BOS system");
                UsersRepository repoUsers = new UsersRepository();
                UsersRequest users = new UsersRequest()
                {
                    users = repoUsers.GetPendingUserToSendToAxonify()
                };
                Console.WriteLine("--------Users to update " + users.users.Count);

                if (users.users.Count > 0)
                {
                    string baseUrl = ConfigurationManager.AppSettings[AppSettings.ApiUrlBase];
                    string fullApiUsers = baseUrl + "users";
                    string jsonParameters = JsonConvert.SerializeObject(users);

                    Console.WriteLine("--------Sending users to Axonify");
                    GeneralResult resultCall = AxonifyApiClient.CallApi(fullApiUsers, HttpMethos.PUT, jsonParameters);

                    if (!string.IsNullOrEmpty(resultCall.content) && resultCall.content.ToLower().Contains("\"status\""))
                    {
                        result = JsonConvert.DeserializeObject<GeneralResult>(resultCall.content);
                        result.content = resultCall.content;
                    }

                    //Remove Areas Of Interest that are not included in BOS system for ACTIVE users
                    Console.WriteLine("--------Removing from Axonify areas of interes that does not are included of each user in BOS system");
                    if (result.status.Equals(ApiStatusResponse.OK, StringComparison.OrdinalIgnoreCase) && users.users != null)
                    {
                        var activeUsers = (from u in users.users where u.active == true select u).ToList();
                        foreach (UsersMod user in activeUsers)
                        {
                            string fullApiAreasOfInterest = baseUrl + "users/" + user.employeeId + "/aois";
                            GeneralResult resultCallAOS = AxonifyApiClient.CallApi(fullApiAreasOfInterest, HttpMethos.GET, string.Empty);
                            Console.WriteLine("------------Getting current areas of interst of user " + user.fullName + " (" + user.employeeId.ToString() + ")");
                            if (resultCallAOS.status.Equals(ApiStatusResponse.OK, StringComparison.OrdinalIgnoreCase))
                            {
                                UsersMod temporalUser = JsonConvert.DeserializeObject<UsersMod>(resultCallAOS.content);
                                string[] currentAxonifyAreas = (temporalUser != null && temporalUser.areasOfInterests != null) ? temporalUser.areasOfInterests : new string[] { };
                                var areasToDelete = currentAxonifyAreas.Except(user.areasOfInterest);

                                Console.WriteLine("------------Removing areas of interest");
                                foreach (var areaOfInterest in areasToDelete)
                                {
                                    string fullApiDeleteAreaOfInterest = baseUrl + "users/" + user.employeeId + "/aois/" + areaOfInterest;
                                    AxonifyApiClient.CallApi(fullApiDeleteAreaOfInterest, HttpMethos.DELETE, string.Empty);
                                }
                            }
                        }
                    }

                    result.statusCode = resultCall.statusCode;
                    result.statusDescription = resultCall.statusDescription;
                }

                InterfacesRepository.InterfaceHistoryUpdate(InterfacesActions.SUCCESSPROCESS, InterfacesNames.AxonifyUsers);
                Console.WriteLine("--------SUCCESS Process");
            }
            catch(Exception ex)
            {
                InterfacesRepository.InterfaceHistoryUpdate(InterfacesActions.FAILPROCESS, InterfacesNames.AxonifyUsers, ex.Message);
                Console.WriteLine("--------FAIL Process");
                Console.WriteLine("--------Error: " + ex.Message);
            }


            Console.WriteLine("----End Interface: " + InterfacesNames.AxonifyUsers);

            return result;
        }
    }
}
