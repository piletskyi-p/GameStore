using System.Globalization;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace GameStore.Web.Controllers.Api
{
    public class BaseApiController : ApiController
    {
        protected string CurrentLangCode { get; set; }

        protected override void Initialize(HttpControllerContext requestContext)
        {
            if (requestContext.RouteData.Values["lang"] != null &&
                requestContext.RouteData.Values["lang"] as string != "null")
            {
                CurrentLangCode = requestContext.RouteData.Values["lang"] as string;
                var ci = new CultureInfo(CurrentLangCode);
                Thread.CurrentThread.CurrentUICulture = ci;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
            }

            base.Initialize(requestContext);
        }
    }
}