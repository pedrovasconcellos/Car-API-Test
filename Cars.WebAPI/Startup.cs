using Cars.WebAPI.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.SwaggerConfigurationExtension;

namespace Cars.WebAPI
{
    public class Startup
    {
        public Startup()
        {
            CarRepository.SetDefault();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => { });

            services.AddApiVersioning(options =>
            {
                options.ApiVersionReader = new QueryStringApiVersionReader();
                options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
                options.ReportApiVersions = true;
            });

            string tokenType = null;
            ApiKeyScheme apiKeyScheme = null;

            string projectName = "Vasconcellos Cars WebAPI";
            string projectDescription = "This project has the purpose of performing an exemplification";

            var swaggerConfigurationExtension = new SwaggerStartupConfigureServices(services, tokenType, apiKeyScheme)
                .SetProjectNameAndDescriptionn(projectName, projectDescription);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            bool withInjectStyleSheet = true;
            string relativePathInjectStyleSheet = "../Stateless/swaggercustom.css";
            string swaggerDocumentationRoute = "Swagger";

            var swaggerStartupConfigure =
                new SwaggerStartupConfigure(app, withInjectStyleSheet, swaggerDocumentationRoute, relativePathInjectStyleSheet).RedirectToSwagger();
        }
    }
}
