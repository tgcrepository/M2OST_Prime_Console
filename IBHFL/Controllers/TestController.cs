using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IBHFL.Controllers
{
    [RoutePrefix("api/Test")]
    public class TestController : ApiController
    {
        [Route("PostTestAPI")]
        [HttpPost]
        public IHttpActionResult PostTestAPI(int Content_Assessment_ID)
        {
            return Json("value");
        }
    }
}
