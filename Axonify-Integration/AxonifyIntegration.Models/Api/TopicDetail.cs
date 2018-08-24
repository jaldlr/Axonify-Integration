using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxonifyIntegration.Models.Api
{
    public class TopicDetail
    {
        public string categoryName { set; get; }
        public string categoryExternalId { set; get; }
        public string subjectName { set; get; }
        public string subjectExternalId { set; get; }
        public string topicName { set; get; }
        public string topicExternalId { set; get; }
        public int level { set; get; }
    }
}