using System.Web.Mvc;
using NLog;

namespace GameStore.Web.Filters
{
    public class ExceptionHandlerAttribute : HandleErrorAttribute
    {
        private readonly ILogger _logger;

        public ExceptionHandlerAttribute()
        {
            _logger = DependencyResolver.Current.GetService<ILogger>();
        }

        public override void OnException(ExceptionContext filterContext)
        {
            _logger.Error(filterContext.Exception);

            base.OnException(filterContext);
        }
    }
}