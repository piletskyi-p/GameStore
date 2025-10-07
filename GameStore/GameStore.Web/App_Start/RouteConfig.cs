using System.Web.Mvc;
using System.Web.Routing;

namespace GameStore.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               "CustomHome",
               "{lang}",
               new { controller = "Game", action = "Filter", lang = "en" });

            routes.MapRoute(
                "Customgames",
                "{lang}/games",
                new { controller = "game", action = "Filter", lang = "en" });

            routes.MapRoute(
                "basketController",
                "{lang}/basket",
                new { Controller = "Order", action = "Basket", lang = "en" });

            routes.MapRoute(
                "HistoryForOrder",
                "{lang}/orders/history",
                new { Controller = "Order", action = "GetAllOrders", lang = "en" });

            routes.MapRoute(
                "OrdersFromSql",
                "{lang}/orders",
                new { Controller = "Order", action = "GetOrdersFromSql", lang = "en" });

            routes.MapRoute(
                name: "defaultNewcommentforgame",
                url: "{lang}/{controller}s/{gamekey}/newcomment",
                defaults: new { action = "LeaveCommentForGame", lang = "en", gamekey = UrlParameter.Optional });

            routes.MapRoute(
                "ControllerForpublisherNew",
                "{lang}/publisher/new",
                new { CompanyName = UrlParameter.Optional, controller = "publisher", action = "new", lang = "en" });

            routes.MapRoute(
                "ControllerForpublisher",
                "{lang}/publisher/{CompanyName}",
                new { CompanyName = UrlParameter.Optional, controller = "publisher", action = "PublisherDetails", lang = "en" });

            routes.MapRoute(
                "Get all comments by key",
                "{lang}/game/{key}/comments",
                new
                {
                    key = UrlParameter.Optional,
                    action = "GetAllCommentsByGameKey",
                    controller = "Comments",
                    lang = "en"
                });

            routes.MapRoute(
                "Download game",
                "{lang}/game/{key}/download",
                new
                {
                    action = "download",
                    controller = "game",
                    lang = "en"
                });

            routes.MapRoute(
                "ControllerWithKey",
                "{lang}/game/{key}",
                new { key = UrlParameter.Optional, controller = "Game", action = "GetGameDetailsByKey", lang = "en" });

            routes.MapRoute(
                "Controllers",
                "{lang}/{controller}s/{action}/{id}",
                new { id = UrlParameter.Optional, action = "Filter", lang = "en" });

            routes.MapRoute(
                "Default",
                "{lang}/{controller}/{action}/{key}",
                new { id = UrlParameter.Optional, lang = "en" });

            routes.MapRoute(
                "ControllerWithGameKey",
                "{lang}/{controller}/{gamekey}/{action}",
                new { gamekey = UrlParameter.Optional, lang = "en" });
        }
    }
}