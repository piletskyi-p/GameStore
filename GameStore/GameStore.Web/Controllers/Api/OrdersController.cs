using System.Net;
using System.Web;
using System.Web.Http;
using GameStore.Bll.Interfaces;

namespace GameStore.Web.Controllers.Api
{
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;

        public OrdersController(IOrderService orderService, IUserService userService)
        {
            _orderService = orderService;
            _userService = userService;
        }

        public IHttpActionResult Get(int id)
        {
            var email = HttpContext.Current.User.Identity.Name;
            var orderDto = _orderService.GetOrderByUserEmail(email);

            if (orderDto == null)
            {
                return NotFound();
            }

            return Content(HttpStatusCode.OK, orderDto);
        }

        [System.Web.Http.Authorize(Roles = "User")]
        public IHttpActionResult Post()
        {
            var email = HttpContext.Current.User.Identity.Name;
            var userId = _userService.GetUser(email).Id;
            _orderService.NewOrderForUser(userId);

            return Content(HttpStatusCode.OK, $"New order for {email}");
        }

        [System.Web.Http.Authorize(Roles = "User")]
        public IHttpActionResult Put(int gameId)
        {
            var email = HttpContext.Current.User.Identity.Name;
            var userId = _userService.GetUser(email).Id;
            _orderService.NewOrderByGameId(gameId, userId);

            return Content(HttpStatusCode.Created, $"New order details for {email}");
        }

        [System.Web.Http.Authorize(Roles = "User")]
        public IHttpActionResult Delete(int orderDetailsId)
        {
            _orderService.RemoveOrderDetails(orderDetailsId);

            return Content(HttpStatusCode.OK, $"Remove order details for");
        }
    }
}