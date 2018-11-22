// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Startup.cs" company="">
//
// </copyright>
// <summary>
//   Defines the Startup type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi
{
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.WebEncoders;
    using NETCore.MailKit.Extensions;
    using NETCore.MailKit.Infrastructure.Internal;
    using System;
    using System.Text.Encodings.Web;
    using System.Text.Unicode;
    using WebApi.EF.Models;
    using WebApi.Infrastructer;

    /// <summary>
    ///     The startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="config">
        /// The config.
        /// </param>
        /// <param name="environment">
        /// The environment.
        /// </param>
        public Startup(IConfiguration config, IHostingEnvironment environment)
        {
            if (environment.IsProduction())
            {
                var builder = new ConfigurationBuilder().SetBasePath(environment.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
                    .AddJsonFile($"MyJson.json", optional: false).AddEnvironmentVariables();

                Configuration = builder.Build();
                config = Configuration;
            }
            else
            {
                Configuration = config;
            }

            Environment = environment;
        }

        /// <summary>
        ///     Gets the configuration.
        /// </summary>
        public IConfiguration Configuration { get; private set; }

        /// <summary>
        ///     Gets the environment.
        /// </summary>
        public IHostingEnvironment Environment { get; }

        /// <summary>
        ///     The configure.
        ///     This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">
        ///     The app.
        /// </param>
        /// <param name="env">
        ///     The env.
        /// </param>
        /// <param name="loggerFactory">
        ///     The logger factory.
        /// </param>
        /// <param name="serviceProvider">
        ///     The service provider.
        /// </param>
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IServiceProvider serviceProvider)
        {
            app.UseCors();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
            app.UseRequestLocalization();
            if (Environment.IsDevelopment())
            {
                SeedData.EnsurePopulated(app);
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
        }

        /// <summary>
        ///     The configure services.
        ///     This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">
        ///     The services.
        /// </param>
        /// <returns>
        ///     The <see cref="IServiceProvider" />.
        /// </returns>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            Console.WriteLine(Configuration["Connection:String"]);

            var tokenParams = TokenValidationParametersBuilder.GetTokenValidationParameters(Configuration);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                config => { config.TokenValidationParameters = tokenParams; });

            services.AddDbContext<EFContext>(
                options =>
                    {
                        if (Environment.IsDevelopment())
                        {
                            options.UseInMemoryDatabase("TestDb");
                        }
                        else
                        options.UseSqlServer(Configuration["Connection:String"], b => b.MigrationsAssembly("WebApi"));
                    });

            services.AddSingleton(tokenParams);

            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.Configure<WebEncoderOptions>(
                options => options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All));

            Console.WriteLine($"{Configuration["Email:Server"]}:{Configuration["Email:Port"]}");

            services.AddMailKit(
                optionBuilder =>
                    {
                        optionBuilder.UseMailKit(
                            new MailKitOptions
                                {
                                    // get options from sercets.json
                                    Server = Configuration["Email:Server"],
                                    Port = Convert.ToInt32(Configuration["Email:Port"]),
                                    SenderName = Configuration["Email:SenderName"],
                                    SenderEmail = Configuration["Email:SenderEmail"],

                                    // can be optional with no authentication
                                    Account = Configuration["Email:Account"],
                                    Password = Configuration["Email:Password"],

                                    // enable ssl or tls
                                    Security = true
                                });
                    });
            return services.BuildServiceProvider();
        }
    }
}