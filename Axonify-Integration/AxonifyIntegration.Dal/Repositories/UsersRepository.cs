using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AxonifyIntegration.Dal.AdoDB;
using AxonifyIntegration.Models.Api;
using AxonifyIntegration.Models.Constants;

namespace AxonifyIntegration.Dal.Repositories
{
    public class UsersRepository
    {
        #region Properties

        private string _connectionString = AxonifyIntegration.Models.Constants.AppSettings.CnnAxonifyIntegration;

        #endregion

        /// <summary>
        /// Get the list of users pending to be sent to axonify
        /// </summary>
        /// <returns>List<UsersMod></returns>
        public List<UsersMod> GetPendingUserToSendToAxonify()
        {
            List<UsersMod> items = new List<UsersMod>();

            using(AdoHelper db = new AdoHelper(this._connectionString))
            {
                db.Connect();

                items.Add(new UsersMod()
                {
                    employeeId = "28",
                    active = true,
                    fullName = "Bob Smith",
                    nickName = "Bobby",
                    username = "bsmith",
                    md5Password = "asd",
                    userType = "ADMIN",
                    email = "myemail@xyzcorp.com",
                    language = "EN",
                    hireDate = "20151123",
                    jobTitle = "Shipper Receiver",
                    department = "Shipping",
                    team = "DC - 1234",
                    lineOfBusiness = "Pharma",
                    areasOfInterest = new string[] { "Safety", "Leadership" },
                    suspended = false
                });

                db.Dispose();
            }

            return items;
        }
    }
}
