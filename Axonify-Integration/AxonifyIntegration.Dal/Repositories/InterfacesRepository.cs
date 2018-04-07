using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AxonifyIntegration.Dal.AdoDB;

namespace AxonifyIntegration.Dal.Repositories
{
    public class InterfacesRepository
    {
        /// <summary>
        /// Manage the history of the executions of each interfaces
        /// </summary>
        /// <returns>List<UsersMod></returns>
        public static void InterfaceHistoryUpdate(string action, string interfaceId, string errorMessage = "")
        {
            using (AdoHelper db = new AdoHelper(AxonifyIntegration.Models.Constants.AppSettings.CnnAxonifyIntegration))
            {
                db.Connect();

                db.ExecDataSetProc("axf_usp_InterfaceHistoryUpdate",
                    "@Action", action,
                    "@InterfaceId", interfaceId,
                    "@ErrorMessage", errorMessage
                );

                db.Dispose();
            }
        }
    }
}
