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

namespace AxonifyIntegration.Dal.ApiClient
{
    public class UsersApiClient
    {
        public GeneralResult AddUsers(UsersRequestcs users)
        {
            GeneralResult result = new GeneralResult();

            if (users != null && users.users != null && users.users.Count > 0)
            {
                string apiFullUrl = ConfigurationManager.AppSettings[AppSettings.ApiUrlBase] + "users";
                string jsonParameters = JsonConvert.SerializeObject(users);
                GeneralResult resultCall = AxonifyApiClient.CallApi(apiFullUrl, "put", "application/json", "application/json", jsonParameters);

                if (!string.IsNullOrEmpty(resultCall.content) && resultCall.content.ToLower().Contains("\"status\""))
                {
                    result = JsonConvert.DeserializeObject<GeneralResult>(resultCall.content);
                    result.content = resultCall.content;
                }

                result.statusCode = resultCall.statusCode;
                result.statusDescription = resultCall.statusDescription;
            }

            return result;
        }
    }
}
