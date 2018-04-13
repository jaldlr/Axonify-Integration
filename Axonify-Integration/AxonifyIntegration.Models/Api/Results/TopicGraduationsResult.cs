using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxonifyIntegration.Models.Api.Results
{
    public class TopicGraduationsResult
    {
        public string timePeriod { set; get; }
        public string timePeriodDate { set; get; }
        public bool hasMore { set; get; }
        public List<TopicGraduation> topicGraduations { set; get; }
        
        public TopicGraduationsResult()
        {
            this.topicGraduations = new List<TopicGraduation>();
        }
    }
}
