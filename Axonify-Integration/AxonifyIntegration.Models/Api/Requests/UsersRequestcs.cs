using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxonifyIntegration.Models.Api.Requests
{
    public class UsersRequestcs
    {
        public List<UsersMod> users { set; get; }

        public UsersRequestcs()
        {
            this.users = new List<UsersMod>();
        }
    }
}
