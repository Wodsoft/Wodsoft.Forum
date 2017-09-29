using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wodsoft.ComBoost.Security;
using Microsoft.EntityFrameworkCore;
using Wodsoft.ComBoost.Data.Entity;
using Microsoft.AspNetCore.Http;
using Wodsoft.ComBoost;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Wodsoft.ComBoost.AspNetCore;
using Wodsoft.ComBoost.Data;
using Wodsoft.Forum.Sample.Domain;
using Wodsoft.ComBoost.Data.Entity.Metadata;
using Wodsoft.Forum.Sample.Entity;

namespace Wodsoft.Forum.Sample
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            _Env = env;
            Configuration = configuration;
        }

        private IHostingEnvironment _Env;
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSession();
            services.AddMvc(options =>
            {
                options.AddComBoostMvcDataOptions();
            });
            services.AddComBoostMvcAuthentication<ComBoostAuthenticationSessionHandler>();

            services.AddScoped<DbContext, DataContext>(serviceProvider =>
                new DataContext(new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase("Test")
                .Options));
            services.AddScoped<IDatabaseContext, DatabaseContext>();
            services.AddScoped<ISecurityProvider, ForumSecurityProvider>();
            services.AddScoped<IAuthenticationProvider, ComBoostAuthenticationProvider>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IDomainServiceAccessor, DomainServiceAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<ICacheProvider, MemoryCacheProvider>();
            services.AddSingleton<ISerializerProvider, JsonSerializerProvider>();
            services.AddScoped<IStorageProvider, PhysicalStorageProvider>(t =>
            {
                return new PhysicalStorageProvider(PhysicalStorageOptions.CreateDefault(_Env.ContentRootPath + System.IO.Path.DirectorySeparatorChar + "Uploads"));
            });
            services.AddSingleton<IDomainServiceProvider, DomainProvider>(t =>
            {
                var provider = new DomainProvider(t);
                provider.AddGenericDefinitionExtension(typeof(EntityDomainService<>), typeof(EntitySearchExtension<>));
                provider.AddGenericDefinitionExtension(typeof(EntityDomainService<>), typeof(EntityPagerExtension<>));
                provider.AddGenericDefinitionExtension(typeof(EntityDomainService<>), typeof(EntityPasswordExtension<>));
                provider.AddGenericDefinitionExtension(typeof(EntityDomainService<>), typeof(ImageExtension<>));
                provider.AddForumExtensions();
                provider.AddGlobalFilter<AliasFilter>();
                return provider;
            });

            EntityDescriptor.InitMetadata(typeof(Board).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseSession();

            app.UseComBoost();

            app.UseComBoostMvc(routes =>
            {
                routes.MapAreaRoute("areaRoute", "Admin", "Admin/{controller=Home}/{action=Index}/{id?}", null, null, new
                {
                    authArea = "Admin",
                    loginPath = "/Admin/Account/SignIn",
                    logoutPath = "/Admin/Account/SignOut"
                });

                routes.MapRoute("default", "{controller=Board}/{action=Index}/{id?}", null, null, new
                {
                    loginPath = "/Account/SignIn",
                    logoutPath = "/Account/SignOut",
                    expireTime = TimeSpan.FromDays(30)
                });
            });
        }
    }
}
