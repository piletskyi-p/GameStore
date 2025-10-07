using System.Web;
using System.Web.Routing;
using Moq;
using NUnit.Framework;

namespace GameStore.Web.Tests.ControllersTests
{
    [TestFixture]
    public class GameControllerUrlTests
    {
        // Create game (POST URL: /games/new). 
        [Test]
        public void New()
        {
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.Setup(httpcon => httpcon.Request.AppRelativeCurrentExecutionFilePath)
                .Returns("~/ru/games/new");

            RouteData routeData = routes.GetRouteData(httpContextMock.Object);

            Assert.IsNotNull(routeData);
            Assert.AreEqual("game", routeData.Values["Controller"]);
            Assert.AreEqual("new", routeData.Values["action"]);
        }

        // Edit game (POST URL: /games/update). 
        [Test]
        public void Update()
        {
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.Setup(httpcon => httpcon.Request.AppRelativeCurrentExecutionFilePath)
                .Returns("~/ru/games/update");

            RouteData routeData = routes.GetRouteData(httpContextMock.Object);

            Assert.IsNotNull(routeData);
            Assert.AreEqual("game", routeData.Values["Controller"]);
            Assert.AreEqual("update", routeData.Values["action"]);
        }

        // Get game details by key (GET URL: /game/{key}).  
        [Test]
        public void GetGameDetailsByKey()
        {
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.Setup(httpcon => httpcon.Request.AppRelativeCurrentExecutionFilePath)
                .Returns("~/ru/game/ME");

            RouteData routeData = routes.GetRouteData(httpContextMock.Object);

            Assert.IsNotNull(routeData);
            Assert.AreEqual("Game", routeData.Values["Controller"]);
            Assert.AreEqual("GetGameDetailsByKey", routeData.Values["action"]);
            Assert.AreEqual("ME", routeData.Values["key"]);
        }

        [Test]
        public void GetGames()
        {
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.Setup(httpcon => httpcon.Request.AppRelativeCurrentExecutionFilePath)
                .Returns("~/ru/games");

            RouteData routeData = routes.GetRouteData(httpContextMock.Object);

            Assert.IsNotNull(routeData);
            Assert.AreEqual("game", routeData.Values["Controller"]);
            Assert.AreEqual("Filter", routeData.Values["action"]);
        }

        // Get all games (GET URL: /games). 
        [Test]
        public void GetAllGames()
        {
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.Setup(httpcon => httpcon.Request.AppRelativeCurrentExecutionFilePath)
                .Returns("~/ru/games");

            RouteData routeData = routes.GetRouteData(httpContextMock.Object);

            Assert.IsNotNull(routeData);
            Assert.AreEqual("game", routeData.Values["Controller"]);
            Assert.AreEqual("Filter", routeData.Values["action"]);
        }

        // Delete game (POST URL: /games/remove). 
        [Test]
        public void DeleteGame()
        {
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.Setup(httpcon => httpcon.Request.AppRelativeCurrentExecutionFilePath)
                .Returns("~/ru/games/remove/1");

            RouteData routeData = routes.GetRouteData(httpContextMock.Object);

            Assert.IsNotNull(routeData);
            Assert.AreEqual("game", routeData.Values["Controller"]);
            Assert.AreEqual("remove", routeData.Values["action"]);
            Assert.AreEqual("1", routeData.Values["id"]);
        }

        // Leave comment for game (POST URL: /games/{gamekey}/newcomment). 
        [Test]
        public void LeaveCommentForGame()
        {
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.Setup(httpcon => httpcon.Request.AppRelativeCurrentExecutionFilePath)
                .Returns("~/ru/games/ME/newcomment");

            RouteData routeData = routes.GetRouteData(httpContextMock.Object);

            Assert.IsNotNull(routeData);
            Assert.AreEqual("game", routeData.Values["Controller"]);
            Assert.AreEqual("LeaveCommentForGame", routeData.Values["action"]);
            Assert.AreEqual("ME", routeData.Values["gamekey"]);
        }

        // Get all comments by game key (GET URL: /game/{gamekey}/comments) 
        [Test]
        public void GetAllCommentsByGameKey()
        {
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.Setup(httpcon => httpcon.Request.AppRelativeCurrentExecutionFilePath)
                .Returns("~/ru/game/ME/comments");

            RouteData routeData = routes.GetRouteData(httpContextMock.Object);

            Assert.IsNotNull(routeData);
            Assert.AreEqual("Comments", routeData.Values["Controller"]);
            Assert.AreEqual("GetAllCommentsByGameKey", routeData.Values["action"]);
            Assert.AreEqual("ME", routeData.Values["key"]);
        }

        // Download game (jut return any binary file as response) (GET URL: game/{gamekey}/download)
        [Test]
        public void DownloadGame()
        {
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.Setup(httpcon => httpcon.Request.AppRelativeCurrentExecutionFilePath)
                .Returns("~/ru/game/ME/download");

            RouteData routeData = routes.GetRouteData(httpContextMock.Object);

            Assert.IsNotNull(routeData);
            Assert.AreEqual("game", routeData.Values["Controller"]);
            Assert.AreEqual("download", routeData.Values["action"]);
            Assert.AreEqual("ME", routeData.Values["key"]);
        }

        // View Publisher Details(GET URL: /publisher/{CompanyName} )
        [Test]
        public void PublisherDetails()
        {
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.Setup(httpcon => httpcon.Request.AppRelativeCurrentExecutionFilePath)
                .Returns("~/ru/publisher/Sony");

            RouteData routeData = routes.GetRouteData(httpContextMock.Object);

            Assert.IsNotNull(routeData);
            Assert.AreEqual("publisher", routeData.Values["Controller"]);
            Assert.AreEqual("PublisherDetails", routeData.Values["action"]);
            Assert.AreEqual("Sony", routeData.Values["CompanyName"]);
        }

        // View New publisher(GET URL: /publisher/new )
        [Test]
        public void PublisherNew()
        {
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.Setup(httpcon => httpcon.Request.AppRelativeCurrentExecutionFilePath)
                .Returns("~/ru/publisher/new");

            RouteData routeData = routes.GetRouteData(httpContextMock.Object);

            Assert.IsNotNull(routeData);
            Assert.AreEqual("publisher", routeData.Values["Controller"]);
            Assert.AreEqual("new", routeData.Values["action"]);
        }

        // GET URL: /basket
        [Test]
        public void Basket()
        {
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.Setup(httpcon => httpcon.Request.AppRelativeCurrentExecutionFilePath)
                .Returns("~/ru/basket");

            RouteData routeData = routes.GetRouteData(httpContextMock.Object);

            Assert.IsNotNull(routeData);
            Assert.AreEqual("Order", routeData.Values["Controller"]);
            Assert.AreEqual("Basket", routeData.Values["action"]);
        }
    }
}
