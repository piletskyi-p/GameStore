using System;
using System.Globalization;
using System.IO;
using System.Web;

namespace GameStore.Web.Handlers
{
    public class ImageHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;

            string imageURL = null;
            if (request.UrlReferrer != null)
            {
                if (String.Compare(request.Url.Host, request.UrlReferrer.Host, true, CultureInfo.InvariantCulture) == 0)
                {
                    imageURL = request.PhysicalPath;
                    if (!File.Exists(imageURL))
                    {
                        response.Status = "Image Not Found";
                        response.StatusCode = 404;
                    }
                    else
                    {
                        response.ContentType =  Path.GetExtension(imageURL).ToLower();
                        response.WriteFile(imageURL);
                        return;
                    }
                }
            }
            if (imageURL == null)
            {
                imageURL = context.Server.MapPath("~/Content/Images/Games/notallowed.png");
            }
            response.ContentType = "~/Content/Images/Games/" + Path.GetExtension(imageURL).ToLower();
            response.WriteFile(imageURL);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}