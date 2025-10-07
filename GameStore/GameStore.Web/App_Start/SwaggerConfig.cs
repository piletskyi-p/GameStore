
using Swashbuckle.Application;
using System.Linq;
using System.Web;

[assembly: PreApplicationStartMethod(typeof(GameStore.Web.SwaggerConfig), "Register")]

namespace GameStore.Web
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            System.Web.Http.GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                    c.SingleApiVersion("v1", "GameStore.Web");

                })
                .EnableSwaggerUi(c =>
                {
 
                });
        }
    }
}
