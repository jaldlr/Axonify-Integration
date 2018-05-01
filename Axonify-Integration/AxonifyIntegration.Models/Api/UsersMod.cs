using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxonifyIntegration.Models.Api
{
    public class UsersMod
    {
        public string employeeId { set; get; }
        public Boolean active { set; get; }
        public string fullName { set; get; }
        public string nickName { set; get; }
        public string username { set; get; }
        public string md5Password { set; get; }
        public string userType { set; get; }
        public string email { set; get; }
        public string language { set; get; }
        public string hireDate { set; get; }
        public DateTime hireDateDate { set; get; }
        public string jobTitle { set; get; }
        public string department { set; get; }
        public string team { set; get; }
        public string lineOfBusiness { set; get; }
        public string AOI { set; get; }
        public string areasOfInterest { set; get; }
        public string quizId { get; set; }
        public string brandId { get; set; }
        public string brandName { get; set; }
        public string[] areasOfInterests { set; get; }
        public Boolean suspended { set; get; }

        public UsersMod()
        {
            //this.areasOfInterest = new string[] { };
        }
    }
}
