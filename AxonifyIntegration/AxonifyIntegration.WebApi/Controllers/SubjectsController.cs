using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using AxonifyIntegration.Data.DataModels;
using AxonifyIntegration.Object.Constants;
using AxonifyIntegration.WebApi.Utilities;

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
                API_Subject tt = null;
                tt.ToString();
                IEnumerable<API_Subject> subjects = (from s in this._dbContext.API_Subject select s).ToList();
                var result = new
                {
                    subjects = subjects
                };
                response = Request.CreateResponse(HttpStatusCode.OK, result);
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

                var result = new
                {
                    subject = subject
                };
                response = Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                response = HttpStatusCodes.statusCode500;
            }
            
            return response;
        }

        // POST: api/Subjects
        public HttpResponseMessage Post(API_Subject value)
        {
            HttpResponseMessage response;

            try
            {
                if (value != null && ModelState.IsValid)
                {
                    API_Subject subject = (from s in this._dbContext.API_Subject where s.subjectExternalId.Equals(value.subjectExternalId, StringComparison.OrdinalIgnoreCase) select s).FirstOrDefault();
                    if (subject == null)
                    {
                        this._dbContext.API_Subject.Add(value);
                    }
                    else
                    {
                        subject.subjectName = value.subjectName;
                        subject.categoryExternalId = value.categoryExternalId;
                        subject.revision = value.revision;
                    }
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
            catch (Exception ex)
            {
                response = HttpStatusCodes.statusCode500;
            }

            return response;
        }

        // PUT: api/Subjects/5
        public HttpResponseMessage Put(string id, API_Subject value)
        {
            try
            {
                if (value != null)
                {
                    value.subjectExternalId = id;
                }

                return this.Post(value);
            }
            catch (Exception ex)
            {
                return HttpStatusCodes.statusCode500;
            }
        }

        #endregion
    }
}
