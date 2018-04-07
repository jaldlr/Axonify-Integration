using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxonifyIntegration.Models.Constants
{
    public class AppSettings
    {
        public static string ApiUrlBase = "Api_UrlBase";
        public static string ApiHeaderTokenName = "Api_HeaderTokenName";
        public static string ApiToken = "Api_Token";
        public static string CnnAxonifyIntegration = "AxonifyIntegration";
    }

    public class ApiStatusResponse
    {
        public static string OK = "OK";
    }

    public class HttpMethos
    {
        public static string GET = "GET";
        public static string POST = "POST";
        public static string PUT = "PUT";
        public static string DELETE = "DELETE";
    }

    public class InterfacesNames
    {
        public static string AxonifyUsers = "AxonifyUsers";
    }

    public class InterfacesActions
    {
        public static string NEWRECORD = "NEWRECORD";
        public static string SUCCESSPROCESS = "SUCCESSPROCESS";
        public static string FAILPROCESS = "FAILPROCESS";
        public static string CANCELPROCESS = "CANCELPROCESS";
    }
}
