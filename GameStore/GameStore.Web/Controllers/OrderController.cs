using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using GameStore.Bll.Interfaces;
using GameStore.Bll.Strategies;
using GameStore.Web.Arrangements;
using GameStore.Web.Auth;
using GameStore.Web.Models.ViewModels;
using GameStore.Web.Pagination;
using NLog;

namespace GameStore.Web.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly IGameService _gameService;
        private readonly IPayService _payService;
        private readonly IShippersService _shippersService;
        private readonly ILogger _logger;

        public OrderController(
            IOrderService orderService,
            IPayService strategy,
            IGameService gameService,
            ILogger logger,
            IUserService userService,
            IShippersService shippersService,
            IAuthentication auth,
            ILanguageService languageService) : base(auth)
        {
            _orderService = orderService;
            _payService = strategy;
            _gameService = gameService;
            _logger = logger;
            _userService = userService;
            _shippersService = shippersService;
        }

        [Authorize(Roles = "User")]
        public ActionResult Buy(string gamekey)
        {
            if (!string.IsNullOrEmpty(gamekey))
            {
                var userEmail = Auth.CurrentUser.Identity.Name;
                var userId = _userService.GetUser(userEmail).Id;
                _orderService.NewOrder(gamekey, userId);
                var order = _orderService.GetOrder(userId);

                if (order != null)
                {
                    var orderViewModel = Mapper.Map<OrderViewModel>(order);

                    return RedirectToAction("GetGameDetailsByKey", "Game", new { key = gamekey });
                }
            }

            _logger.Warn("Game wasn't bought.");

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult Basket()
        {
            var userEmail = Auth.CurrentUser.Identity.Name;
            var userId = _userService.GetUser(userEmail).Id;
            var order = _orderService.GetOrder(userId);

            if (order != null)
            {
                var orderViewModel = Mapper.Map<OrderViewModel>(order);

                return View("Basket", orderViewModel);
            }

            return View("Basket", new OrderViewModel());
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult Order()
        {
            var userEmail = Auth.CurrentUser.Identity.Name;
            var userId = _userService.GetUser(userEmail).Id;
            var order = _orderService.GetOrder(userId);

            if (order != null)
            {
                var orderViewModel = Mapper.Map<OrderViewModel>(order);

                return View("Order", orderViewModel);
            }

            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public FileStreamResult PayBank()
        {
            var userEmail = Auth.CurrentUser.Identity.Name;
            var userId = _userService.GetUser(userEmail).Id;
            var order = _orderService.GetOrder(userId);
            var games = _gameService.GetAllGames().ToList();

            _payService.SetStrategy(new BankStrategy());
            var result = _payService.Pay(order);

            if (result != null)
            {
                WorkWithFiles file = new WorkWithFiles();

                return file.CreateFilePdf(result, games);
            }

            return null;
        }

        [Authorize(Roles = "User")]
        public ActionResult IBox()
        {
            var userEmail = Auth.CurrentUser.Identity.Name;
            var userId = _userService.GetUser(userEmail).Id;
            var order = _orderService.GetOrder(userId);

            _payService.SetStrategy(new IBoxStrategy());
            var result = _payService.Pay(order);

            return View("IBox", result);
        }

        [Authorize(Roles = "User")]
        public ActionResult PayIBox()
        {
            var userEmail = Auth.CurrentUser.Identity.Name;
            var userId = _userService.GetUser(userEmail).Id;
            var order = _orderService.GetOrder(userId);
            _orderService.PayOrder(order);

            return RedirectToAction(nameof(Congratulation));
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult Congratulation()
        {
            return View("Congratulation");
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult VisaView()
        {
            return View("VisaView");
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public ActionResult PayVisa()
        {
            var userEmail = Auth.CurrentUser.Identity.Name;
            var userId = _userService.GetUser(userEmail).Id;
            var order = _orderService.GetOrder(userId);

            _payService.SetStrategy(new VisaStrategy());
            var result = _payService.Pay(order);

            _orderService.PayOrder(result);

            return View("Congratulation");
        }

        [Authorize(Roles = "User")]
        public ActionResult RemoveFromBasket(int idorderdetails)
        {
            _orderService.RemoveOrderDetails(idorderdetails);
            _logger.Info("User remove game from basket.");

            return RedirectToAction(nameof(Basket));
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public ActionResult GetAllOrders(int page = 1)
        {
            var dateFrom = DateTime.MinValue;
            var dateTo = DateTime.UtcNow.AddDays(-30);
            var ordersDto = _orderService.ReturnInRange(dateFrom, dateTo);
            var orders = Mapper.Map<List<OrderViewModel>>(ordersDto);

            int pageSize = 60;
            IEnumerable<OrderViewModel> ordersPerPages = orders.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = orders.Count() };
            var ordersView = new EmptyOrdersView
            {
                PageInfo = pageInfo,
                Orders = ordersPerPages.ToList(),
                DateFrom = dateFrom,
                DateTo = dateTo
            };

            return View("AllOrders", ordersView);
        }

        public ActionResult OrderFilter(DateTime dateFrom, DateTime dateTo, int page = 1)
        {
            const int pageSize = 60;
            if (dateFrom <= dateTo)
            {
                var ordersDto = _orderService.ReturnInRange(dateFrom, dateTo).ToList();
                if (ordersDto.Any())
                {
                    var orders = Mapper.Map<List<OrderViewModel>>(ordersDto);
                    var ordersPerPages = orders.Skip((page - 1) * pageSize).Take(pageSize);

                    var pageInfo = new PageInfo
                    {
                        PageNumber = page,
                        PageSize = pageSize,
                        TotalItems = orders.Count
                    };

                    var ordersView = new EmptyOrdersView
                    {
                        PageInfo = pageInfo,
                        Orders = ordersPerPages.ToList(),
                        DateFrom = dateFrom,
                        DateTo = dateTo
                    };

                    return View("AllOrders", ordersView);
                }
                else
                {
                    var ordersView = new EmptyOrdersView
                    {
                        PageInfo = new PageInfo(),
                        Orders = new List<OrderViewModel>(),
                        DateFrom = dateFrom,
                        DateTo = dateTo
                    };

                    return View("AllOrders", ordersView);
                }
            }

            var ordersViewNull = new EmptyOrdersView
            {
                PageInfo = new PageInfo(),
                Orders = new List<OrderViewModel>()
            };

            return View("AllOrders", ordersViewNull);
        }

        [Authorize(Roles = "Manager")]
        public ActionResult SetShipped(int id)
        {
            _orderService.SetShipped(id);

            return RedirectToAction("GetOrdersFromSql");
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public ActionResult GetOrdersFromSql(int page = 1)
        {
            var dateFrom = DateTime.UtcNow.AddDays(-30);
            var dateTo = DateTime.UtcNow;
            var ordersDto = _orderService.ReturnInRangeFromSQL(dateFrom, dateTo).ToList();
            var orders = Mapper.Map<IEnumerable<OrderViewModel>>(ordersDto);

            int pageSize = 60;
            IEnumerable<OrderViewModel> ordersPerPages = orders.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = orders.Count() };
            var ordersView = new EmptyOrdersView()
            {
                PageInfo = pageInfo,
                Orders = ordersPerPages.ToList(),
                DateFrom = dateFrom,
                DateTo = dateTo
            };

            return View("OrdersSQL", ordersView);
        }

        public ActionResult GetShippers()
        {
            var shippersDto = _shippersService.Get();
            var shippers = Mapper.Map<IEnumerable<ShippersViewModel>>(shippersDto);

            return View("Shippers", shippers);
        }

        [Authorize(Roles = "Manager")]
        public ActionResult OrderFilterSQL(DateTime DateFrom, DateTime DateTo, int page = 1)
        {
            if (DateFrom <= DateTo)
            {
                var ordersDto = _orderService.ReturnInRangeFromSQL(DateFrom, DateTo).ToList();
                if (ordersDto.Any())
                {
                    var orders = Mapper.Map<List<OrderViewModel>>(ordersDto);
                    int pageSize = 60;
                    IEnumerable<OrderViewModel> ordersPerPages = orders.Skip((page - 1) * pageSize).Take(pageSize);
                    PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = orders.Count() };
                    var ordersView = new EmptyOrdersView()
                    {
                        PageInfo = pageInfo,
                        Orders = ordersPerPages.ToList(),
                        DateFrom = DateFrom,
                        DateTo = DateTo
                    };

                    return View("OrdersSQL", ordersView);
                }
            }

            var ordersViewNull = new EmptyOrdersView()
            {
                PageInfo = new PageInfo(),
                Orders = new List<OrderViewModel>(),
                DateFrom = DateFrom,
                DateTo = DateTo
            };

            return View("AllOrders", ordersViewNull);
        }
    }
}