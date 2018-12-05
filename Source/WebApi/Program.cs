// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="">
//   
// </copyright>
// <summary>
//   The program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi
{
    #region

    using System;
    using System.IO;
    using System.Linq;

    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Logging;

    using Newtonsoft.Json;

    using NLog.Web;

    using Swashbuckle.AspNetCore.Swagger;

    #endregion

    /// <summary>
    ///     The program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The create web host builder.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// The <see cref="IWebHostBuilder"/>.
        /// </returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args).UseStartup<Startup>().ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                    })
                .UseNLog();
        }

        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        public static void Main(string[] args)
        {
            var path = Environment.GetCommandLineArgs()[0];

            var dir = Path.GetDirectoryName(path);

            Directory.SetCurrentDirectory(dir);

            var build = CreateWebHostBuilder(args).Build();

            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            if (args.Length > 0  && args.Contains("swagger"))
            {
                logger.Debug(GenerateSwagger(build, args[1]));
                }

            try
            {
                build.Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        /// <summary>
        /// The generate swagger.
        /// </summary>
        /// <param name="host">
        /// The host.
        /// </param>
        /// <param name="docName">
        /// The doc name.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string GenerateSwagger(IWebHost host, string docName)
        {
            var sw = (ISwaggerProvider)host.Services.GetService(typeof(ISwaggerProvider));
            var doc = sw.GetSwagger(docName, null, "/");

            using (var writer = new StringWriter())
            {
                writer.Write(
                    JsonConvert.SerializeObject(
                        doc,
                        Formatting.Indented,
                        new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore,
                                ContractResolver = new SwaggerContractResolver(new JsonSerializerSettings())
                            }));

                var file = AppDomain.CurrentDomain.BaseDirectory + "swagger.json";
                using (var stream = new StreamWriter(file))
                {
                    stream.WriteLine(writer.ToString());
                }
            }

            return "Swagger Created";
        }
    }
}