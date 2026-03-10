using System.Web.Http;
using Swashbuckle.Application;

namespace SqlVersionService.Web.App_Start
{
    public static class SwaggerConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "SQL Version Service");
                    c.PrettyPrint();
                    c.DescribeAllEnumsAsStrings();
                })
                .EnableSwaggerUi();
        }
    }
}