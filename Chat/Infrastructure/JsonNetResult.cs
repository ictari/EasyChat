using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chat.Infrastructure
{
    public class JsonNetResult : JsonResult
    {
        public JsonNetResult()
        {
            JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var response = context.HttpContext.Response;

            response.ContentType = !String.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            var setting = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var serializedObject = JsonConvert.SerializeObject(Data, Formatting.None, setting);
            response.Write(serializedObject);
        }
    }
}