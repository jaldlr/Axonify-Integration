using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxonifyIntegration.Models.Api.Results
{
    public class IntroductoryCompletionsResult
    {
        public string timePeriod { set; get; }
        public string timePeriodDate { set; get; }
        public bool hasMore { set; get; }
        public List<IntroductoryCompletion> introductoryCompletions { set; get; }

        public IntroductoryCompletionsResult()
        {
            this.introductoryCompletions = new List<IntroductoryCompletion>();
        }
    }
}