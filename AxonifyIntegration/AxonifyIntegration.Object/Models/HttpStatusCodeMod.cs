using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxonifyIntegration.Object.Models
{
    public class HttpStatusCodeMod
    {
        #region Properties

        public int codeNumber { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public Boolean isSuccess { get; set; }

        #endregion
    }
}
