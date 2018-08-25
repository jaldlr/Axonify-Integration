using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxonifyIntegration.Models.Api
{
    public class IntroductoryCompletion
    {
        public string employeeId { set; get; }
        public string completionTimestamp { set; get; }
        public double assessmentScore { set; get; }
        public int timeSpent { set; get; }
        public TopicDetail topicDetails { set; get; }
    }
}