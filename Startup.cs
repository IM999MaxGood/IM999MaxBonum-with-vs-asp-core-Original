using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

//999/for using session
using IM999MaxBonum.Models;

//999/for using resource
//using System.Globalization;
//using Microsoft.AspNetCore.Localization;
//using Microsoft.AspNetCore.Mvc.Localization;
//using Microsoft.AspNetCore.Mvc.Razor;

//999/for using ef
using IM999MaxBonum.Data;
using Microsoft.EntityFrameworkCore;

//999/for encrypt and decrypt
using System.Security.Cryptography;
using Microsoft.AspNetCore.DataProtection;

using System.IO;

//using Microsoft.Extensions.FileProviders.Physical;
//using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
//using System.Web.Hosting;

namespace IM999MaxBonum
{
    public class Startup
    {
        private readonly IHostingEnvironment Environment;

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
    
            Environment = environment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //999/for iis deploy and publish
            services.Configure<IISOptions>(options =>
            {
                options.ForwardClientCertificate = false;
            });

            //999/for getting client ip address in 'BaseController'  
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            /*
            //999/for using resource files .resx    
            services.AddLocalization(o => o.ResourcesPath = "Resources");
            
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    //new CultureInfo("en-GB"),
                    new CultureInfo("fa-IR")
                };
                options.DefaultRequestCulture = new RequestCulture("fa-IR", "fa-IR");

                // You must explicitly state which cultures your application supports.
                // These are the cultures the app supports for formatting 
                // numbers, dates, etc.
                options.SupportedCultures = supportedCultures;

                // These are the cultures the app supports for UI strings, 
                // i.e. we have localized resources for.
                options.SupportedUICultures = supportedCultures;

                options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(async context =>
                {
                    // My custom request culture logic
                    return new ProviderCultureResult("fa");
                }));

            });
            */

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //999/ for using ef sql server
            services.AddDbContext<IM999Max_DotNetCore>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc()
                /*.AddViewLocalization(
                    LanguageViewLocationExpanderFormat.Suffix,
                    opts => { opts.ResourcesPath = "Resources"; })
                .AddDataAnnotationsLocalization()*/
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


            //999/for using session
            // Adds a default in-memory implementation of IDistributedCache.
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.Name = ".IM999MaxBonum._";
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.IdleTimeout = TimeSpan.FromMinutes(10);
            });

            //var tempFolder = Path.Combine(Environment.ContentRootPath, "~/");
            //var tempFolder = Path.Combine(Environment.WebRootPath, "~/");
            //var tempFolder = Path.Combine(Directory.GetCurrentDirectory(), "~/");
            var tempFolder = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "~/");

            


            //999/encrypt and decrypt    
            //services.AddDataProtection(opt => opt.ApplicationDiscriminator = "im999maxbonum")
            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(tempFolder))
                //.PersistKeysToFileSystem(new DirectoryInfo("~/"))     //@"c:\"))     //@"\\server\share\directory\"))
                                                                      //.ProtectKeysWithCertificate("thumbprint") "3BCE558E2AD3E0E34A7743EAB5AEA2A9BD2575A0"
                .SetDefaultKeyLifetime(TimeSpan.FromDays(7));
            //.SetApplicationName("IM999Max.com") //999/ for sharing key between two app
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            //999/for iis
            app.UseDefaultFiles();

            //999 must correct !!!   
            //if (HostingEnvironment.IsHosted)
            //{
                try{
                    app.UseStaticFiles(new StaticFileOptions()
                    {
                        FileProvider = new PhysicalFileProvider($@"{AppDomain.CurrentDomain.BaseDirectory}/wwwroot")
                        //FileProvider = new PhysicalFile(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "/wwwroot"), "text/html")
                    });

                }catch{

                }
            //}


            //if (env.IsDevelopment())
            //{
            app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //999/ظاهرا برای خطاست
            //    app.UseExceptionHandler("/Home/Error");
            //    app.UseHsts();
            //}

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            //999/for using session
            app.UseSession();

            app.UseMvc(routes =>
            {

                //999/ for captcha image   
                routes.MapRoute(
                    name: "General",
                    template: "General/CaptchaImage",
                    defaults: new { controller = "General", action = "CaptchaImage" });


                //999/ for using area
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{lang=ir}/{controller=Home}/{action=Index}/{id?}");

                /*
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                */

                routes.MapRoute(
                    name: "English",
                    template: "en/{controller}/{action}/{id?}",
                    defaults: new { lang = "en", controller = "Home", action = "Index" }
                );

                routes.MapRoute(
                    name: "Default",
                    template: "{lang}/{controller}/{action}/{id?}",
                    defaults: new { lang = "ir", controller = "Home", action = "Index" }
                );

            });
        }
    }
}
