using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace IM999MaxBonum
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            //WebHost.CreateDefaultBuilder(args)
            //    .UseStartup<Startup>();


           WebHost.CreateDefaultBuilder(args)

                //im999 for log errors  
                .CaptureStartupErrors(true)
                .UseSetting("detailedErrors", "true")
                
                //.UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                //.UseUrls("https://localhost:5001", "http://192.168.1.99:80")
                
                //.UseIISIntegration()
                .UseIIS()

                .UseStartup<Startup>();

/*
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) 
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            //var builder = WebHost.CreateDefaultBuilder(args);
            var builder = WebHost.CreateDefaultBuilder(args).UseContentRoot(Directory.GetCurrentDirectory());

            if (env == EnvironmentName.Staging || env == EnvironmentName.Production)
                builder.UseIIS();

            builder.UseStartup<Startup>();
            return builder;
        }
*/
/*
            var host = new WebHostBuilder()
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseUrls("http://localhost:5000", "http://odin:5000", "http://192.168.1.2:5000")
            .UseIISIntegration()
            .UseStartup<Startup>()
            .Build();
*/
    }
}
