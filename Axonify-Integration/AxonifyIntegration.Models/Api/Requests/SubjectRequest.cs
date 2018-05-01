using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxonifyIntegration.Models.Api.Requests
{
    public class SubjectRequest
    {
        public List<Subject> subjects { set; get; }

        public SubjectRequest()
        {
            this.subjects = new List<Subject>();
        }
    }
}
