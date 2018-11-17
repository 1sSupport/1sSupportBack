namespace WebApi
{
    #region

    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;

    #endregion

    /// <summary>
    ///     The program.
    /// </summary>
    public class Program
    {
        /// <summary>
        ///     The create web host builder.
        /// </summary>
        /// <param name="args">
        ///     The args.
        /// </param>
        /// <returns>
        ///     The <see cref="IWebHostBuilder" />.
        /// </returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
        }

        /// <summary>
        ///     The main.
        /// </summary>
        /// <param name="args">
        ///     The args.
        /// </param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }
    }
}