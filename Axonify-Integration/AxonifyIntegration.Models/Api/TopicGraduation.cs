using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxonifyIntegration.Models.Api
{
    public class TopicGraduation
    {
        public string employeeId { set; get; }
        public string graduationTimestamp { set; get; }
        public double baselineAverage { set; get; }
        public int baselineCorrectAnswerCount { set; get; }
        public int baselineAnswerCount { set; get; }
        public double currentAverage { set; get; }
        public int currentCorrectAnswerCount { set; get; }
        public int currentAnswerCount { set; get; }
        public double overallAverage { set; get; }
        public int overallCorrectAnswerCount { set; get; }
        public int overallAnswerCount { set; get; }
        public int timeSpent { set; get; }
        public TopicDetail topicDetails { set; get; }
    }
}