using System.Globalization;
using System.Net;
using System.Threading;
using System.Web.Http;
using System.Web.Mvc;
using GameStore.Bll.Auth;
using GameStore.Bll.DTO;
using GameStore.Web.Auth;

namespace GameStore.Web.Controllers
{
    public class BaseController : Controller
    {
        protected BaseController(IAuthentication auth)
        {
            Auth = auth;
        }

        public UserDTO CurrentUser => ((UserIndentity)Auth.CurrentUser.Identity).User;

        public string CurrentLangCode { get; protected set; }

        protected IAuthentication Auth { get; set; }
        
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            if (requestContext.RouteData.Values["lang"] != null &&
                requestContext.RouteData.Values["lang"] as string != "null")
            {
                CurrentLangCode = requestContext.RouteData.Values["lang"] as string;
                if (CurrentLangCode != "ru" && CurrentLangCode != "en")
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }
                var ci = new CultureInfo(CurrentLangCode);
                Thread.CurrentThread.CurrentUICulture = ci;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
            }

            base.Initialize(requestContext);
        }
    }
}