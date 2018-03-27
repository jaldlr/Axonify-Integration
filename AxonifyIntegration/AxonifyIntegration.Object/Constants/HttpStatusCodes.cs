using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AxonifyIntegration.Object.Models;

namespace AxonifyIntegration.Object.Constants
{
    public class HttpStatusCodes
    {
        public static HttpStatusCodeMod statusCode200 = new HttpStatusCodeMod()
        {
            codeNumber = 200,
            message = "OK",
            description = "Everything is working",
            isSuccess = true
        };

        public static HttpStatusCodeMod statusCode304 = new HttpStatusCodeMod()
        {
            codeNumber = 304,
            message = "Not Modified",
            description = "No changes were have been made to the target resource",
            isSuccess = false
        };

        public static HttpStatusCodeMod statusCode400 = new HttpStatusCodeMod()
        {
            codeNumber = 400,
            message = "Bad Request",
            description = "The request was invalid or cannot be served. See error payload for details",
            isSuccess = false
        };

        public static HttpStatusCodeMod statusCode401 = new HttpStatusCodeMod()
        {
            codeNumber = 401,
            message = "Unauthorized",
            description = "The request requires an user authentication",
            isSuccess = false
        };

        public static HttpStatusCodeMod statusCode403 = new HttpStatusCodeMod()
        {
            codeNumber = 403,
            message = "Forbidden",
            description = "The server has refused the request or the access is not allowed. See error payload for details",
            isSuccess = false
        };

        public static HttpStatusCodeMod statusCode404 = new HttpStatusCodeMod()
        {
            codeNumber = 404,
            message = "Not found",
            description = "There is no resource behind the URI",
            isSuccess = false
        };

        public static HttpStatusCodeMod statusCode422 = new HttpStatusCodeMod()
        {
            codeNumber = 422,
            message = "Unprocessable Entity",
            description = "Mandatory fields missing from payload. See error payload for details",
            isSuccess = false
        };

        public static HttpStatusCodeMod statusCode429 = new HttpStatusCodeMod()
        {
            codeNumber = 429,
            message = "Too many requests",
            description = "",
            isSuccess = false
        };

        public static HttpStatusCodeMod statusCode500 = new HttpStatusCodeMod()
        {
            codeNumber = 500,
            message = "Internal Server Error",
            description = "See error payload for details",
            isSuccess = false
        };

        public static HttpStatusCodeMod statusCode503 = new HttpStatusCodeMod()
        {
            codeNumber = 503,
            message = "Service Unavailable",
            description = "Used to indicate maintenance",
            isSuccess = false
        };
    }
}
