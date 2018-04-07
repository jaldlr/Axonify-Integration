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
            GeneralResult result = new GeneralResult();

            try
            {
                UsersRepository repoUsers = new UsersRepository();
                UsersRequest users = new UsersRequest()
                {
                    users = repoUsers.GetPendingUserToSendToAxonify()
                };

                if (users != null && users.users != null && users.users.Count > 0)
                {
                    string baseUrl = ConfigurationManager.AppSettings[AppSettings.ApiUrlBase];
                    string fullApiUsers = baseUrl + "users";
                    string jsonParameters = JsonConvert.SerializeObject(users);
                    GeneralResult resultCall = AxonifyApiClient.CallApi(fullApiUsers, HttpMethos.PUT, jsonParameters);

                    if (!string.IsNullOrEmpty(resultCall.content) && resultCall.content.ToLower().Contains("\"status\""))
                    {
                        result = JsonConvert.DeserializeObject<GeneralResult>(resultCall.content);
                        result.content = resultCall.content;
                    }

                    //Remove Areas Of Interest that are not included in BOS system for ACTIVE users
                    if (result.status.Equals(ApiStatusResponse.OK, StringComparison.OrdinalIgnoreCase) && users.users != null)
                    {
                        var activeUsers = (from u in users.users where u.active == true select u).ToList();
                        foreach (UsersMod user in activeUsers)
                        {
                            string fullApiAreasOfInterest = baseUrl + "users/" + user.employeeId + "/aois";
                            GeneralResult resultCallAOS = AxonifyApiClient.CallApi(fullApiAreasOfInterest, HttpMethos.GET, string.Empty);
                            if (resultCallAOS.status.Equals(ApiStatusResponse.OK, StringComparison.OrdinalIgnoreCase))
                            {
                                UsersMod temporalUser = JsonConvert.DeserializeObject<UsersMod>(resultCallAOS.content);
                                string[] currentAxonifyAreas = (temporalUser != null && temporalUser.areasOfInterests != null) ? temporalUser.areasOfInterests : new string[] { };
                                var areasToDelete = currentAxonifyAreas.Except(user.areasOfInterest);

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
            }
            catch(Exception ex)
            {
                InterfacesRepository.InterfaceHistoryUpdate(InterfacesActions.FAILPROCESS, InterfacesNames.AxonifyUsers, ex.Message);
            }

            return result;
        }
    }
}
