using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using AxonifyIntegration.Data.DataModels;
using AxonifyIntegration.Object.Constants;

namespace AxonifyIntegration.WebApi.Controllers
{
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
            IEnumerable<API_Subject> subjects = (from s in this._dbContext.API_Subject select s).ToList();
            var result = new
            {
                subjects = subjects
            };
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        // GET: api/Subjects/5
        public HttpResponseMessage Get(string id)
        {
            API_Subject subject = (from s in this._dbContext.API_Subject where s.subjectExternalId.Equals(id, StringComparison.OrdinalIgnoreCase) select s).FirstOrDefault();
            
            var result = new
            {
                subject = subject
            };
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        // POST: api/Subjects
        public HttpResponseMessage Post(API_Subject value)
        {
            if (value != null && ModelState.IsValid)
            {
                API_Subject subject = (from s in this._dbContext.API_Subject where s.subjectExternalId.Equals(value.subjectExternalId, StringComparison.OrdinalIgnoreCase) select s).FirstOrDefault();
                if(subject == null)
                {
                    this._dbContext.API_Subject.Add(value);
                }else
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
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, new Exception(HttpStatusCodes.statusCode400.message));
            }
        }

        // PUT: api/Subjects/5
        public HttpResponseMessage Put(string id, API_Subject value)
        {
            if(value != null)
            {
                value.subjectExternalId = id;
            }

            return this.Post(value);
        }

        #endregion
    }
}
