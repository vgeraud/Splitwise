using Newtonsoft.Json;
using Splitwise.Model.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace Splitwise.Helpers
{
    public static class ResponseHelper
    {
        public static HttpResponseMessage ResponseFromInvalidParametersException(InvalidParametersException invalidParams)
        {
            var simplifiedObject = new { Message = invalidParams.Message, InvalidParameters = invalidParams.ParameterErrorCollection };

            var exceptionSerialized = JsonConvert.SerializeObject(
                simplifiedObject,
                Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });
            
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            response.Content = new StringContent(exceptionSerialized);
            return response;
        }
    }
}