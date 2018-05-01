using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxonifyIntegration.Models.Api
{
    public class Subject
    {
        public string categoryExternalId { get; set; }
        public string subjectExternalId { get; set; }
        public string subjectName { get; set; }
        public string revision { get; set; }
    }
}
