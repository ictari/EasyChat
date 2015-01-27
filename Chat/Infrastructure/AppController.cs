using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Chat.Infrastructure
{
    public class AppController : Controller
    {

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult { Data = data, ContentType = contentType, JsonRequestBehavior = behavior };
        }
    }
}