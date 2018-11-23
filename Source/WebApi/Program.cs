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

    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;

    using Newtonsoft.Json;

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
            return WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
        }

        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        public static void Main(string[] args)
        {
            var build = CreateWebHostBuilder(args).Build();

            if (args.Length > 0 && args[0] == "swagger" && args[1] != string.Empty)
            {
                Console.WriteLine(GenerateSwagger(build, args[1]));
                build.Run();
            }
            else
            {
                build.Run();
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