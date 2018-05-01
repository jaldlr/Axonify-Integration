using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxonifyIntegration.Models.Api
{
    public class Topic
    {
        public string subjectExternalId { get; set; }
        public string topicExternalId { get; set; }
        public string topicName { get; set; }
        public string revision { get; set; }
    }
}
