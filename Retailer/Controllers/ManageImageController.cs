using Retailer.Common;
using Retailer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Microsoft.AspNet.Identity;

namespace Retailer.Controllers
{
    public class ManageImageController : ApiController
    {
      

        [HttpPost]
        [Route("AddImage")]
        public async Task<HttpResponseMessage> AddImage(string folder)
        {
            string UserId=User.Identity.GetUserId();
            if(String.IsNullOrEmpty(folder))
            return Request.CreateResponse<ResponseModel<string>>( new ResponseModel<string> { Status = HttpStatusCode.BadRequest,  Message = "Destination not passed(folder name)" });
            string images = Utils.AddImage(folder +"/"+ UserId);
            if(String.IsNullOrEmpty(images))
            return Request.CreateResponse<ResponseModel<string>>( new ResponseModel<string> { Status = HttpStatusCode.BadRequest,  Message = "Something Went Wrong" });
            
            return Request.CreateResponse<ResponseModel<string>>( new ResponseModel<string> { Status = HttpStatusCode.OK, Data=images, Message = "Uploaded" });
        }
    }
}