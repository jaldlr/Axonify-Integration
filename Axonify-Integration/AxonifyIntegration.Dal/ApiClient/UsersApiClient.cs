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
        /// Call an Axonify api to create or update users in Axonify
        /// </summary>
        public void SendPendingUsers()
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
            catch (Exception ex)
            {
                InterfacesRepository.InterfaceHistoryUpdate(InterfacesActions.FAILPROCESS, InterfacesNames.AxonifyUsers, ex.Message);
                Console.WriteLine("--------FAIL Process");
                Console.WriteLine("--------Error: " + ex.Message);
            }


            Console.WriteLine("----End Interface: " + InterfacesNames.AxonifyUsers);
        }

        /// <summary>
        /// Call an Axonify api to get all topic graduations from current date, and register them into BOS system
        /// </summary>
        public void GetTopicGraduations()
        {
            InterfacesRepository.InterfaceHistoryUpdate(InterfacesActions.NEWRECORD, InterfacesNames.TopicGraduations);
            Console.WriteLine("----Execution Interface: " + InterfacesNames.TopicGraduations);
            TopicGraduationsResult topicGraduationsResult = new TopicGraduationsResult();
            List<TopicGraduation> topicGraduations = new List<TopicGraduation>();

            try
            {
                string timePeriod = "DAY";
                string timePeriodDate = DateTime.Now.ToString("yyyyMMdd");
                string baseUrl = ConfigurationManager.AppSettings[AppSettings.ApiUrlBase];
                string fullApiUsers = baseUrl + "users/topicGraduations?timePeriod=" + timePeriod + "&timePeriodDate=" + timePeriodDate + "&page=";
                int page = 0;

                do
                {
                    Console.WriteLine("--------Getting topics graduations for [timePeriod = " + timePeriod + "; timePeriodDate = " + timePeriodDate + "; page = " + page.ToString() + ";]");
                    GeneralResult resultCall = AxonifyApiClient.CallApi(fullApiUsers + page.ToString(), HttpMethos.GET, string.Empty);
                    topicGraduationsResult = new TopicGraduationsResult();

                    Console.WriteLine("-------------HttpStatusCode = " + resultCall.statusCode.ToString());
                    if (resultCall.statusCode == HttpStatusCode.OK)
                    {
                        topicGraduationsResult = JsonConvert.DeserializeObject<TopicGraduationsResult>(resultCall.content);
                        topicGraduations.AddRange(topicGraduationsResult.topicGraduations);
                    }
                    else
                    {
                        topicGraduationsResult.hasMore = false;
                    }
                    ++page;
                } while (topicGraduationsResult != null && topicGraduationsResult.hasMore);

                if (topicGraduations.Count > 0)
                {
                    Console.WriteLine("--------Updating in BOS system topic graduations for " + topicGraduations.Count.ToString() + " users");
                    UsersRepository repository = new UsersRepository();
                    repository.UpdateTopicGraduations(topicGraduations);
                }
                else
                {
                    Console.WriteLine("--------No topic graduations to be updated");
                }

                InterfacesRepository.InterfaceHistoryUpdate(InterfacesActions.SUCCESSPROCESS, InterfacesNames.TopicGraduations);
                Console.WriteLine("--------SUCCESS Process");
            }
            catch (Exception ex)
            {
                InterfacesRepository.InterfaceHistoryUpdate(InterfacesActions.FAILPROCESS, InterfacesNames.TopicGraduations, ex.Message);
                Console.WriteLine("--------FAIL Process");
                Console.WriteLine("--------Error: " + ex.Message);
            }


            Console.WriteLine("----End Interface: " + InterfacesNames.TopicGraduations);
        }
    }
}
