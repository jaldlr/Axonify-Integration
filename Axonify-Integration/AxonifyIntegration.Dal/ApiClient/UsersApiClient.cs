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
                    string fullApiUrl = baseUrl + "users";

                    int perPage = 100;
                    List<UsersMod> paginatedUsers = (from u in users.users select u).Take(perPage).ToList();
                    while(paginatedUsers != null && paginatedUsers.Count > 0)
                    {
                        UsersRequest usersToSend = new UsersRequest()
                        {
                            users = paginatedUsers
                        };

                        string jsonParameters = JsonConvert.SerializeObject(usersToSend);

                        Console.WriteLine("--------Sending users to Axonify");
                        GeneralResult resultCall = AxonifyApiClient.CallApi(fullApiUrl, HttpMethos.PUT, jsonParameters);

                        if (!string.IsNullOrEmpty(resultCall.content) && resultCall.content.ToLower().Contains("\"status\""))
                        {
                            result = JsonConvert.DeserializeObject<GeneralResult>(resultCall.content);
                            result.content = resultCall.content;
                        }

                        //Remove Areas Of Interest that are not included in BOS system for ACTIVE users
                        //JLUNA: August 23 2018: This block of code is not necessary because Axonify automatically remove the areas of interes that were not included inside "areasOfInterest" parameter of the API that was called before this
                        
                        /*Console.WriteLine("--------Removing from Axonify areas of interes that does not are included of each user in BOS system");
                        if (result.status.Equals(ApiStatusResponse.OK, StringComparison.OrdinalIgnoreCase) && usersToSend.users != null)
                        {
                            var activeUsers = (from u in usersToSend.users where u.active == true select u).ToList();
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
                        }*/
                        

                        result.statusCode = resultCall.statusCode;
                        result.statusDescription = resultCall.statusDescription;

                        if (resultCall.statusCode == HttpStatusCode.OK)
                        {
                            users.users.RemoveAll(x => paginatedUsers.Any(y => y.employeeId == x.employeeId));
                            paginatedUsers = (from u in users.users select u).Take(perPage).ToList();
                        }else
                        {
                            paginatedUsers = null;
                        }
                    }
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
                string fullApiUrl = baseUrl + "users/topicGraduations?timePeriod=" + timePeriod + "&timePeriodDate=" + timePeriodDate + "&page=";
                int page = 1;

                do
                {
                    Console.WriteLine("--------Getting topics graduations for [timePeriod = " + timePeriod + "; timePeriodDate = " + timePeriodDate + "; page = " + page.ToString() + ";]");
                    GeneralResult resultCall = AxonifyApiClient.CallApi(fullApiUrl + page.ToString(), HttpMethos.GET, string.Empty);
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
                    Console.WriteLine("--------No topic graduations were found in axonify");
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

        /// <summary>
        /// Call an Axonify api to get all introductory completions from current date, and register them into BOS system
        /// </summary>
        public void GetIntroductoryCompletions()
        {
            InterfacesRepository.InterfaceHistoryUpdate(InterfacesActions.NEWRECORD, InterfacesNames.IntroductoryCompletions);
            Console.WriteLine("----Execution Interface: " + InterfacesNames.IntroductoryCompletions);
            IntroductoryCompletionsResult introductoryCompletionsResult = new IntroductoryCompletionsResult();
            List<IntroductoryCompletion> introductoryCompletions = new List<IntroductoryCompletion>();

            try
            {
                string timePeriod = "MONTH";
                string timePeriodDate = DateTime.Now.ToString("yyyyMMdd");
                string baseUrl = ConfigurationManager.AppSettings[AppSettings.ApiUrlBase];
                string fullApiUrl = baseUrl + "users/introductoryCompletions?timePeriod=" + timePeriod + "&timePeriodDate=" + timePeriodDate + "&page=";
                int page = 1;

                do
                {
                    Console.WriteLine("--------Getting introductory completions for [timePeriod = " + timePeriod + "; timePeriodDate = " + timePeriodDate + "; page = " + page.ToString() + ";]");
                    GeneralResult resultCall = AxonifyApiClient.CallApi(fullApiUrl + page.ToString(), HttpMethos.GET, string.Empty);
                    introductoryCompletionsResult = new IntroductoryCompletionsResult();

                    Console.WriteLine("-------------HttpStatusCode = " + resultCall.statusCode.ToString());
                    if (resultCall.statusCode == HttpStatusCode.OK)
                    {
                        introductoryCompletionsResult = JsonConvert.DeserializeObject<IntroductoryCompletionsResult>(resultCall.content);
                        introductoryCompletions.AddRange(introductoryCompletionsResult.introductoryCompletions);
                    }
                    else
                    {
                        introductoryCompletionsResult.hasMore = false;
                    }
                    ++page;
                } while (introductoryCompletionsResult != null && introductoryCompletionsResult.hasMore);


                //<JLUNA SIMULATION>

                introductoryCompletions.Add(new IntroductoryCompletion()
                {
                    employeeId = "107",
                    completionTimestamp = "20180824T20:11-05:00",
                    assessmentScore = 11,
                    timeSpent = 51,
                    topicDetails = new TopicDetail()
                    {
                        categoryName = "categoryName1",
                        categoryExternalId = "categoryExternalId1",
                        subjectName = "subjectName1",
                        subjectExternalId = "subjectExternalId1",
                        topicName = "topicName1",
                        topicExternalId = "583",
                        level = 21
                    }
                });

                introductoryCompletions.Add(new IntroductoryCompletion()
                {
                    employeeId = "107",
                    completionTimestamp = "20180824T20:12-05:00",
                    assessmentScore = 12,
                    timeSpent = 52,
                    topicDetails = new TopicDetail()
                    {
                        categoryName = "categoryName2",
                        categoryExternalId = "categoryExternalId2",
                        subjectName = "subjectName2",
                        subjectExternalId = "subjectExternalId2",
                        topicName = "topicName2",
                        topicExternalId = "9",
                        level = 22
                    }
                });

                introductoryCompletions.Add(new IntroductoryCompletion()
                {
                    employeeId = "107",
                    completionTimestamp = "20180824T20:13-05:00",
                    assessmentScore = 13,
                    timeSpent = 53,
                    topicDetails = new TopicDetail()
                    {
                        categoryName = "categoryName3",
                        categoryExternalId = "categoryExternalId3",
                        subjectName = "subjectName3",
                        subjectExternalId = "subjectExternalId3",
                        topicName = "topicName3",
                        topicExternalId = "8",
                        level = 23
                    }
                });

                //</JLUNA SIMULATION>

                if (introductoryCompletions.Count > 0)
                {
                    Console.WriteLine("--------Updating in BOS system introductory completions for " + introductoryCompletions.Count.ToString() + " users");
                    UsersRepository repository = new UsersRepository();
                    repository.UpdateIntroductoryCompletions(introductoryCompletions);
                }
                else
                {
                    Console.WriteLine("--------No introductory completions wer found in axonify");
                }

                InterfacesRepository.InterfaceHistoryUpdate(InterfacesActions.SUCCESSPROCESS, InterfacesNames.IntroductoryCompletions);
                Console.WriteLine("--------SUCCESS Process");
            }
            catch (Exception ex)
            {
                InterfacesRepository.InterfaceHistoryUpdate(InterfacesActions.FAILPROCESS, InterfacesNames.IntroductoryCompletions, ex.Message);
                Console.WriteLine("--------FAIL Process");
                Console.WriteLine("--------Error: " + ex.Message);
            }


            Console.WriteLine("----End Interface: " + InterfacesNames.IntroductoryCompletions);
        }
    }
}