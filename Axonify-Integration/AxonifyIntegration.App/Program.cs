using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AxonifyIntegration.Models.Api;
using AxonifyIntegration.Models.Api.Requests;
using AxonifyIntegration.Models.Api.Results;
using AxonifyIntegration.Dal.Repositories;
using AxonifyIntegration.Dal.ApiClient;
using AxonifyIntegration.Models.Constants;

namespace AxonifyIntegration.App
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args != null)
            {
                UsersApiClient usersApiClient = new UsersApiClient();
                //usersApiClient.GetTopicGraduations();
                switch (args[0].ToString())
                {
                    case "AxonifyUsers":
                        usersApiClient.SendPendingUsers();
                        break;
                    case "TopicGraduations":
                        usersApiClient.GetTopicGraduations();
                        break;
                }
            }
        }
    }
}
