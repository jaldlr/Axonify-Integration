using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;

using AxonifyIntegration.Object.Models;

namespace AxonifyIntegration.Object.Constants
{
    public class HttpStatusCodes
    {
        public static HttpResponseMessage statusCode200 = new HttpResponseMessage((HttpStatusCode)200)
        {
            Content = new StringContent("Everything is working."),
            ReasonPhrase = "OK"
        };

        public static HttpResponseMessage statusCode304 = new HttpResponseMessage((HttpStatusCode)304)
        {
            Content = new StringContent("No changes were have been made to the target resource."),
            ReasonPhrase = "Not Modified"
        };

        public static HttpResponseMessage statusCode400 = new HttpResponseMessage((HttpStatusCode)400)
        {
            Content = new StringContent("The request was invalid or cannot be served."),
            ReasonPhrase = "Bad Request"
        };

        public static HttpResponseMessage statusCode401 = new HttpResponseMessage((HttpStatusCode)401)
        {
            Content = new StringContent("The request requires an user authentication."),
            ReasonPhrase = "Unauthorized"
        };

        public static HttpResponseMessage statusCode403 = new HttpResponseMessage((HttpStatusCode)403)
        {
            Content = new StringContent("The server has refused the request or the access is not allowed."),
            ReasonPhrase = "Forbidden"
        };

        public static HttpResponseMessage statusCode404 = new HttpResponseMessage((HttpStatusCode)404)
        {
            Content = new StringContent("There is no resource behind the URI."),
            ReasonPhrase = "Not found"
        };

        public static HttpResponseMessage statusCode422 = new HttpResponseMessage((HttpStatusCode)422)
        {
            Content = new StringContent("Mandatory fields missing from request."),
            ReasonPhrase = "Unprocessable Entity"
        };

        public static HttpResponseMessage statusCode429 = new HttpResponseMessage((HttpStatusCode)429)
        {
            Content = new StringContent(""),
            ReasonPhrase = "Too many requests"
        };

        public static HttpResponseMessage statusCode500 = new HttpResponseMessage((HttpStatusCode)500)
        {
            Content = new StringContent("See error payload for details."),
            ReasonPhrase = "Internal Server Error"
        };

        public static HttpResponseMessage statusCode503 = new HttpResponseMessage((HttpStatusCode)503)
        {
            Content = new StringContent("Server in maintenance."),
            ReasonPhrase = "Service Unavailable"
        };
    }
}
