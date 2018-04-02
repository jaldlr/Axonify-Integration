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
    public class TopicsController : ApiController
    {
        #region Properties

        AxonifyIntegrationEntities _dbContext { set; get; }

        #endregion

        #region constructors

        public TopicsController()
        {
            this._dbContext = new AxonifyIntegrationEntities();
        }

        #endregion

        #region Api Methods

        // GET: api/Topics
        [HttpGet]
        public HttpResponseMessage Get()
        {
            HttpResponseMessage response;

            try
            {
                IEnumerable<API_Topic> topics = (from t in this._dbContext.API_Topic select t).ToList();
                response = Request.CreateResponse(HttpStatusCode.OK, topics);
            }
            catch(Exception ex)
            {
                response = HttpStatusCodes.statusCode500;
            }

            return response;
        }

        // GET: api/Topics/5
        [HttpGet]
        public HttpResponseMessage Get(string id)
        {
            HttpResponseMessage response;

            try
            {
                API_Topic topic = (from s in this._dbContext.API_Topic where s.topicExternalId.Equals(id, StringComparison.OrdinalIgnoreCase) select s).FirstOrDefault();
                response = Request.CreateResponse(HttpStatusCode.OK, topic);
            }
            catch (Exception ex)
            {
                response = HttpStatusCodes.statusCode500;
            }
            
            return response;
        }

        // POST: api/Topics
        [HttpPost]
        public HttpResponseMessage Post(List<API_Topic> topics)
        {
            HttpResponseMessage response;
            Boolean isValid = true;
            try
            {
                if (topics != null)
                {
                    foreach(API_Topic tp in topics)
                    {
                        ICollection<ValidationResult> results = null;
                        isValid = Validator.TryValidateObject(tp, new ValidationContext(tp), results, true);
                        if (isValid)
                        {
                            API_Topic topic = (from s in this._dbContext.API_Topic where s.topicExternalId.Equals(tp.topicExternalId, StringComparison.OrdinalIgnoreCase) select s).FirstOrDefault();
                            if (topic == null)
                            {
                                this._dbContext.API_Topic.Add(tp);
                            }
                            else
                            {
                                topic.subjectExternalId = tp.subjectExternalId;
                                topic.topicName = tp.topicName;
                                topic.revision = tp.revision;
                            }
                        }else
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

        // PUT: api/Topics
        [HttpPut]
        public HttpResponseMessage Put(List<API_Topic> topics)
        {
            try
            {
                return this.Post(topics);
            }
            catch (Exception ex)
            {
                return HttpStatusCodes.statusCode500;
            }
        }

        #endregion
    }
}
