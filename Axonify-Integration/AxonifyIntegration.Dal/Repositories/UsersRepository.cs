using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using AxonifyIntegration.Dal.AdoDB;
using AxonifyIntegration.Models.Api;
using AxonifyIntegration.Models.Helpers;
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
            List<UsersMod> users = new List<UsersMod>();

            using(AdoHelper db = new AdoHelper(this._connectionString))
            {
                db.Connect();

                DataSet ds = db.ExecDataSetProc("axf_usp_InterfaceGetPendingUsers");

                if(ds.Tables.Count > 0)
                {
                    users = ds.Tables[0].ToList<UsersMod>();
                    foreach (var user in users)
                    {
                        if (string.IsNullOrEmpty(user.md5Password))
                        {
                            user.md5Password = null;
                        }
                    }
                }

                if (ds.Tables.Count > 0)
                {
                    List<UserAreasOfInterest> areas = ds.Tables[1].ToList<UserAreasOfInterest>();
                    foreach(UsersMod user in users)
                    {
                        user.areasOfInterest = (
                            from a in areas where a.employeeId == user.employeeId
                            select a.areaOfInterest
                        ).ToArray();
                    }
                }

                db.Dispose();
            }

            return users;
        }

        /// <summary>
        /// Update all topic graduations obtained by axonify into BOS system
        /// </summary>
        public void UpdateTopicGraduations(List<TopicGraduation> topics)
        {
            List<UsersMod> users = new List<UsersMod>();

            using (AdoHelper db = new AdoHelper(this._connectionString))
            {
                db.Connect();

                DataSet ds = db.ExecDataSetProc("axf_usp_InterfaceUpdateTopicGraduations",
                    "@TopicGraduationsXML", StringHelper.ObjectToXML(topics)
                );

                db.Dispose();
            }
        }
    }
}
