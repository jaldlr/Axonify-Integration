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
        
        [HttpGet]
        [Route("users")]
        public HttpResponseMessage GetAll()
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
        
        [HttpPut]
        [Route("users")]
        public HttpResponseMessage AddUsers(List<API_User> users)
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

        [HttpGet]
        [Route("users/{employee_id}/aois")]
        public HttpResponseMessage GetAreasOfInteres(string employee_id)
        {
            HttpResponseMessage response;

            try
            {
                API_User user = (from s in this._dbContext.API_User where s.employeeId.Equals(employee_id, StringComparison.OrdinalIgnoreCase) select s).FirstOrDefault();
                List<string> areasOfInterest = new List<string>();
                if(user != null)
                {
                    areasOfInterest = (from t in user.API_UserAreasOfInterest select t.areaOfInterest).ToList();
                }
                var result = new
                {
                    areasOfInterests = areasOfInterest
                };
                response = Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                response = HttpStatusCodes.statusCode500;
            }

            return response;
        }

        [HttpPut]
        [Route("users/{employee_id}/aois/{aoi_to_add}")]
        public HttpResponseMessage AddAreaOfInteres(string employee_id, string aoi_to_add)
        {
            HttpResponseMessage response;

            try
            {
                API_User user = (from s in this._dbContext.API_User where s.employeeId.Equals(employee_id, StringComparison.OrdinalIgnoreCase) select s).FirstOrDefault();

                if (user != null && !string.IsNullOrEmpty(aoi_to_add) && (from t in user.API_UserAreasOfInterest where t.areaOfInterest.Equals(aoi_to_add) select t).FirstOrDefault() == null)
                {
                    user.API_UserAreasOfInterest.Add(new API_UserAreasOfInterest()
                    {
                        employeeId = user.employeeId,
                        areaOfInterest = aoi_to_add
                    });
                    this._dbContext.SaveChanges();
                }

                var result = new
                {
                    status = "OK"
                };
                response = Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                response = HttpStatusCodes.statusCode500;
            }

            return response;
        }

        [HttpDelete]
        [Route("users/{employee_id}/aois/{aoi_to_delete}")]
        public HttpResponseMessage RemoveAreaOfInteres(string employee_id, string aoi_to_delete)
        {
            HttpResponseMessage response;

            try
            {
                API_UserAreasOfInterest area = (
                    from s in this._dbContext.API_UserAreasOfInterest
                    where s.employeeId.Equals(employee_id, StringComparison.OrdinalIgnoreCase) && s.areaOfInterest.Equals(aoi_to_delete, StringComparison.OrdinalIgnoreCase)
                    select s
                ).FirstOrDefault();

                if (area != null)
                {
                    this._dbContext.API_UserAreasOfInterest.Remove(area);
                    this._dbContext.SaveChanges();
                }

                var result = new
                {
                    status = "OK"
                };
                response = Request.CreateResponse(HttpStatusCode.OK, result);
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
