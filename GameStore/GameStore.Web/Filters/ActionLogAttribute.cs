using System.Diagnostics;
using System.Web.Mvc;
using NLog;

namespace GameStore.Web.Filters
{
    public class ActionLogAttribute : FilterAttribute, IActionFilter
    {
        private readonly ILogger _logger;
        private readonly Stopwatch _sw;

        public ActionLogAttribute()
        {
            _logger = DependencyResolver.Current.GetService<ILogger>(); 
            _sw = new Stopwatch();
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            _sw.Stop();
            _logger.Info($"IP: {filterContext.RequestContext.HttpContext.Request.UserHostAddress} | " +
                $"Time span: {_sw.ElapsedMilliseconds}");
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _sw.Start();
        }
    }
} 