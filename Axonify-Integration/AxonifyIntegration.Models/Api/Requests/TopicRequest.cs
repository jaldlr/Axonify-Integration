using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxonifyIntegration.Models.Api.Requests
{
    public class TopicRequest
    {
        public List<Topic> topics { set; get; }

        public TopicRequest()
        {
            this.topics = new List<Topic>();
        }
    }
}
