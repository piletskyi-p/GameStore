using System.Web.Http;
using GameStore.Web.Auth.Api;

namespace GameStore.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.MessageHandlers.Add(new AuthenticationHandler());

            config.Routes.MapHttpRoute(
                "LoginApi",
                "api/login",
                new { controller = "Logins", action = "login" });

            config.Routes.MapHttpRoute(
                "ForCommets",
                "api/{lang}/games/{gameId}/comments/{commentId}",
                new { controller = "comments", commentId = RouteParameter.Optional, lang = "en" });

            config.Routes.MapHttpRoute(
                "GetListByID",
                "api/{lang}/{controller}/{id}/{action}",
                new { action = "GetDetails", lang = "en" });

            //config.Routes.MapHttpRoute(
            //    "DefaultApi",
            //    "api/{lang}/{controller}/{id}",
            //    new
            //    { id = RouteParameter.Optional, lang = "en" });
        }
    }
}