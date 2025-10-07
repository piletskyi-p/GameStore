using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using NUnit.Framework;

namespace GameStore.Web.Tests.ControllersTests.Api
{
    public class ApiUrlTests
    {
        [Test]
        public void LoginApi()
        {
            RouteCollection routes = new RouteCollection();
            routes.MapRoute(
                "LoginApi",
                "api/login",
                new { controller = "Logins", action = "login" });
            RouteConfig.RegisterRoutes(routes);
            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.Setup(httpcon => httpcon.Request.AppRelativeCurrentExecutionFilePath)
                .Returns("~/api/login");

            RouteData routeData = routes.GetRouteData(httpContextMock.Object);

            Assert.IsNotNull(routeData);
            Assert.AreEqual("Logins", routeData.Values["Controller"]);
            Assert.AreEqual("login", routeData.Values["action"]);
        }

        [Test]
        public void ForCommets()
        {
            RouteCollection routes = new RouteCollection();
            routes.MapRoute(
                "ForCommets",
                "api/{lang}/games/{gameId}/comments/{commentId}",
                new
                {
                    controller = "comments",
                    commentId = RouteParameter.Optional,
                    lang = "en"
                });
            RouteConfig.RegisterRoutes(routes);
            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.Setup(httpcon => httpcon.Request.AppRelativeCurrentExecutionFilePath)
                .Returns("~/api/ru/games/1/comments/2");

            RouteData routeData = routes.GetRouteData(httpContextMock.Object);

            Assert.IsNotNull(routeData);
            Assert.AreEqual("comments", routeData.Values["Controller"]);
            Assert.AreEqual("1", routeData.Values["gameId"]);
            Assert.AreEqual("2", routeData.Values["commentId"]);
            Assert.AreEqual("ru", routeData.Values["lang"]);
        }

        [Test]
        public void GetListById()
        {
            RouteCollection routes = new RouteCollection();
            routes.MapRoute(
                "GetListByID",
                "api/{lang}/{controller}/{id}/{action}",
                new { action = "GetDetails", lang = "en" });
            RouteConfig.RegisterRoutes(routes);
            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.Setup(httpcon => httpcon.Request.AppRelativeCurrentExecutionFilePath)
                .Returns("~/api/ru/games/1");

            RouteData routeData = routes.GetRouteData(httpContextMock.Object);

            Assert.IsNotNull(routeData);
            Assert.AreEqual("games", routeData.Values["Controller"]);
            Assert.AreEqual("GetDetails", routeData.Values["Action"]);
            Assert.AreEqual("1", routeData.Values["id"]);
            Assert.AreEqual("ru", routeData.Values["lang"]);
        }

        [Test]
        public void GetListById_WithAction()
        {
            RouteCollection routes = new RouteCollection();
            routes.MapRoute(
                "GetListByID",
                "api/{lang}/{controller}/{id}/{action}",
                new { action = "GetDetails", lang = "en" });
            RouteConfig.RegisterRoutes(routes);
            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.Setup(httpcon => httpcon.Request.AppRelativeCurrentExecutionFilePath)
                .Returns("~/api/ru/games/1/genres");

            RouteData routeData = routes.GetRouteData(httpContextMock.Object);

            Assert.IsNotNull(routeData);
            Assert.AreEqual("games", routeData.Values["Controller"]);
            Assert.AreEqual("genres", routeData.Values["Action"]);
            Assert.AreEqual("1", routeData.Values["id"]);
            Assert.AreEqual("ru", routeData.Values["lang"]);
        }

        [Test]
        public void DefaultApi()
        {
            RouteCollection routes = new RouteCollection();
            routes.MapRoute(
                "DefaultApi",
                "api/{lang}/{controller}/{id}",
                new { id = RouteParameter.Optional, lang = "en" });
            RouteConfig.RegisterRoutes(routes);
            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.Setup(httpcon => httpcon.Request.AppRelativeCurrentExecutionFilePath)
                .Returns("~/api/ru/games/1");

            RouteData routeData = routes.GetRouteData(httpContextMock.Object);

            Assert.IsNotNull(routeData);
            Assert.AreEqual("games", routeData.Values["Controller"]);
            Assert.AreEqual("1", routeData.Values["id"]);
            Assert.AreEqual("ru", routeData.Values["lang"]);
        }

        [Test]
        public void DefaultApi_WithoutId()
        {
            RouteCollection routes = new RouteCollection();
            routes.MapRoute(
                "DefaultApi",
                "api/{lang}/{controller}/{id}",
                new { id = RouteParameter.Optional, lang = "en" });
            RouteConfig.RegisterRoutes(routes);
            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.Setup(httpcon => httpcon.Request.AppRelativeCurrentExecutionFilePath)
                .Returns("~/api/ru/games");

            RouteData routeData = routes.GetRouteData(httpContextMock.Object);

            Assert.IsNotNull(routeData);
            Assert.AreEqual("games", routeData.Values["Controller"]);
            Assert.AreEqual("ru", routeData.Values["lang"]);
        }
    }
}