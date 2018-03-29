using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using AxonifyIntegration.WebApi.Utilities;

namespace AxonifyIntegration.WebApi.Controllers
{
    [AxonifyAuthorization]
    public class UsersController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return Content("Hola mundo");
        }

        public ActionResult Get()
        {
            return Content("Hola mundo 2");
        }
    }
}
