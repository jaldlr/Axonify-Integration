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
    public class SubjectsController : ApiController
    {
        #region Properties

        AxonifyIntegrationEntities _dbContext { set; get; }

        #endregion

        #region constructors

        public SubjectsController()
        {
            this._dbContext = new AxonifyIntegrationEntities();
        }

        #endregion

        #region Api Methods
        // GET: api/Subjects
        public HttpResponseMessage Get()
        {
            HttpResponseMessage response;

            try
            {
                IEnumerable<API_Subject> subjects = (from s in this._dbContext.API_Subject select s).ToList();
                response = Request.CreateResponse(HttpStatusCode.OK, subjects);
            }
            catch(Exception ex)
            {
                response = HttpStatusCodes.statusCode500;
            }

            return response;
        }

        // GET: api/Subjects/5
        public HttpResponseMessage Get(string id)
        {
            HttpResponseMessage response;

            try
            {
                API_Subject subject = (from s in this._dbContext.API_Subject where s.subjectExternalId.Equals(id, StringComparison.OrdinalIgnoreCase) select s).FirstOrDefault();
                response = Request.CreateResponse(HttpStatusCode.OK, subject);
            }
            catch (Exception ex)
            {
                response = HttpStatusCodes.statusCode500;
            }
            
            return response;
        }

        // POST: api/Subjects
        public HttpResponseMessage Post(List<API_Subject> subjects)
        {
            HttpResponseMessage response;
            Boolean isValid = true;
            try
            {
                if (subjects != null)
                {
                    foreach (API_Subject sb in subjects)
                    {
                        ICollection<ValidationResult> results = null;
                        isValid = Validator.TryValidateObject(sb, new ValidationContext(sb), results, true);
                        if (isValid)
                        {
                            API_Subject subject = (from s in this._dbContext.API_Subject where s.subjectExternalId.Equals(sb.subjectExternalId, StringComparison.OrdinalIgnoreCase) select s).FirstOrDefault();
                            if (subject == null)
                            {
                                this._dbContext.API_Subject.Add(sb);
                            }
                            else
                            {
                                subject.subjectName = sb.subjectName;
                                subject.categoryExternalId = sb.categoryExternalId;
                                subject.revision = sb.revision;
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

        // PUT: api/Subjects/5
        public HttpResponseMessage Put(List<API_Subject> subjects)
        {
            try
            {
                return this.Post(subjects);
            }
            catch (Exception ex)
            {
                return HttpStatusCodes.statusCode500;
            }
        }

        #endregion
    }
}
