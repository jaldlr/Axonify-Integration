using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace AxonifyIntegration.Models.Api.Results
{
    public class GeneralResult
    {
        public string status { set; get; }
        public string message { set; get; }
        public string content { set; get; }
        public HttpStatusCode statusCode { set; get; }
        public string statusDescription { set; get; }

        public GeneralResult()
        {
            this.status = "OK";
            this.statusCode = HttpStatusCode.OK;
        }
    }
}