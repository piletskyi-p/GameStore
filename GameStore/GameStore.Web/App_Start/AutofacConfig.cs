using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using GameStore.Bll.Infrastructure;
using GameStore.Web.Auth;
using NLog;

namespace GameStore.Web
{
    public class AutofacConfig
    {
        public static void ConfigureContainer(HttpConfiguration config)
        {
            // get a copy of the container
            var builder = new ContainerBuilder();
            var autoFac = new AutofacConfigBll();

            // register the controller in the current assembly
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            autoFac.ConfigureContainer(builder);
            builder.Register(i => LogManager.GetLogger("*")).As<ILogger>();
            //builder.RegisterType<GenresController>().InstancePerRequest();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // InstancePerRequest() - don't create new object in range of request
            builder.RegisterType<Authentication>().As<IAuthentication>().InstancePerRequest();

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}