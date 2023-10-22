using Autofac;
using Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WebFramework.Configuration;
using WebFramework.CustomMapping;
//using WebFramework.Middlewares;
using WebFramework.Swagger;
using WebFramework.CustomMapping;
using Common;
using WebFramework.Middlewares;

namespace MyApi
{
    public class Startup
    {
        private readonly SiteSettings _siteSetting;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            AutoMapperConfiguration.InitializeAutoMapper();
            _siteSetting = configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //services.AddAutoMapper(config =>
            //{
            //    config.AddCustomMappingProfile();
            //});

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options
                    .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddCustomIdentity(_siteSetting.IdentitySettings);

            services.AddControllers(options =>
            {
                //options.Filters.Add(new AuthorizeFilter()); 

            }).AddNewtonsoftJson(option =>
            {
                option.SerializerSettings.Converters.Add(new StringEnumConverter());
                option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //option.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                //option.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });

            services.AddCustomApiVersioning();

            services.AddSwagger();

            services.AddJwtAuthentication(_siteSetting.JwtSettings);

        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //Register Services to Autofac ContainerBuilder
            builder.AddServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.IntializeDatabase();

            app.UseCustomExceptionHandler();

            app.UseHsts(env);

            app.UseHttpsRedirection();

            app.UseSwaggerAndUI();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //Use this config just in Develoment (not in Production)
            //app.UseCors(config => config.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            //app.UseRouting();
            app.UseEndpoints(config =>
            {
                config.MapControllers(); // Map attribute routing
                                         //    //    .RequireAuthorization(); Apply AuthorizeFilter as global filter to all endpoints
                                         //    //config.MapDefaultControllerRoute(); // Map default route {controller=Home}/{action=Index}/{id?}
            });

            ////Using 'UseMvc' to configure MVC is not supported while using Endpoint Routing.
            ////To continue using 'UseMvc', please set 'MvcOptions.EnableEndpointRouting = false' inside 'ConfigureServices'.
            //app.UseMvc();
        }
    }
}
