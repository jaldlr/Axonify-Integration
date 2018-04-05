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

namespace AxonifyIntegration.App
{
    class Program
    {
        static void Main(string[] args)
        {
            UsersRepository usersRepository = new UsersRepository();
            UsersApiClient usersApiClient = new UsersApiClient();

            List<UsersMod> users = usersRepository.GetPendingUserToSendToAxonify();
            usersApiClient.AddUsers(new UsersRequestcs()
            {
                users = users
            });
        }
    }
}
