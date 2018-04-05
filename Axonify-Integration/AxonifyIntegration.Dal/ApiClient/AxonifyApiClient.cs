using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Configuration;
using Newtonsoft.Json;
using System.IO;

using AxonifyIntegration.Models.Api;
using AxonifyIntegration.Models.Api.Requests;
using AxonifyIntegration.Models.Api.Results;
using AxonifyIntegration.Models.Constants;

namespace AxonifyIntegration.Dal.ApiClient
{
    public class AxonifyApiClient
    {
        public static GeneralResult CallApi(string apiFullUrl, string method, string accept, string contentType, string jsonParameters)
        {
            GeneralResult result = new GeneralResult();
            var http = (HttpWebRequest)WebRequest.Create(new Uri(apiFullUrl));
            http.Accept = accept;
            http.ContentType = contentType;
            http.Method = method;
            http.Headers.Add(ConfigurationManager.AppSettings[AppSettings.ApiHeaderTokenName], ConfigurationManager.AppSettings[AppSettings.ApiToken]);
            
            ASCIIEncoding encoding = new ASCIIEncoding();
            Byte[] bytes = encoding.GetBytes(jsonParameters);

            Stream newStream = http.GetRequestStream();
            newStream.Write(bytes, 0, bytes.Length);
            newStream.Close();

            var response = http.GetResponse();

            var stream = response.GetResponseStream();
            var sr = new StreamReader(stream);
            var content = sr.ReadToEnd();

            result.content = content;
            result.statusCode = ((HttpWebResponse)response).StatusCode;
            result.statusDescription = ((HttpWebResponse)response).StatusDescription;

            return result;
        }
    }
}
