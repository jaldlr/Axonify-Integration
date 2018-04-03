using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using AxonifyIntegration.Data.DataModels;
using AxonifyIntegration.Object.Constants;
using AxonifyIntegration.WebApi.Utilities;
using System.ComponentModel.DataAnnotations;

namespace AxonifyIntegration.WebApi.Controllers
{
    [AxonifyAuthorization]
    [AxonifyExceptionHandler]
    public class UsersController : ApiController
    {
        #region Properties

        AxonifyIntegrationEntities _dbContext { set; get; }

        #endregion

        #region constructors

        public UsersController()
        {
            this._dbContext = new AxonifyIntegrationEntities();
        }

        #endregion

        #region Api Methods

        // GET: Users
        [HttpGet]
        public HttpResponseMessage Get()
        {
            HttpResponseMessage response;

            try
            {
                IEnumerable<API_User> items = (from s in this._dbContext.API_User select s).ToList();
                foreach(API_User item in items)
                {
                    item.areasOfInterest = (from t in item.API_UserAreasOfInterest select t.areaOfInterest).ToList();
                }
                response = Request.CreateResponse(HttpStatusCode.OK, items);
            }
            catch (Exception ex)
            {
                response = HttpStatusCodes.statusCode500;
            }

            return response;
        }

        // Put: Users
        [HttpPut]
        public HttpResponseMessage Put(List<API_User> users)
        {
            HttpResponseMessage response;
            Boolean isValid = true;

            try
            {
                if (users != null)
                {
                    foreach (API_User item in users)
                    {
                        ICollection<ValidationResult> results = null;
                        isValid = Validator.TryValidateObject(item, new ValidationContext(item), results, true);
                        if (isValid)
                        {
                            foreach (string area in item.areasOfInterest)
                            {
                                item.API_UserAreasOfInterest.Add(
                                    new API_UserAreasOfInterest()
                                    {
                                        employeeId = item.employeeId,
                                        areaOfInterest = area
                                    }
                                );
                            }

                            API_User element = (from s in this._dbContext.API_User where s.employeeId.Equals(item.employeeId, StringComparison.OrdinalIgnoreCase) select s).FirstOrDefault();
                            if (element == null)
                            {
                                this._dbContext.API_User.Add(item);
                            }
                            else
                            {
                                element.active = item.active;
                                element.fullName = item.fullName;
                                element.nickName = item.nickName;
                                element.userName = item.userName;
                                element.md5Password = item.md5Password;
                                element.userType = item.userType;
                                element.email = item.email;
                                element.language = item.language;
                                element.hireDate = item.hireDate;
                                element.jobTitle = item.jobTitle;
                                element.department = item.department;
                                element.team = item.team;
                                element.lineOfBusiness = item.lineOfBusiness;
                                element.suspended = item.suspended;
                                element.API_UserAreasOfInterest.Clear();
                                element.API_UserAreasOfInterest = item.API_UserAreasOfInterest;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (isValid)
                    {
                        this._dbContext.SaveChanges();
                        var result = new
                        {
                            status = "OK"
                        };
                        response = Request.CreateResponse(HttpStatusCode.OK, result);
                    }
                    else
                    {
                        response = HttpStatusCodes.statusCode422;
                    }
                }
                else
                {
                    response = HttpStatusCodes.statusCode422;
                }
            }
            catch (Exception ex)
            {
                response = HttpStatusCodes.statusCode500;
            }

            return response;
        }

        #endregion
    }
}
