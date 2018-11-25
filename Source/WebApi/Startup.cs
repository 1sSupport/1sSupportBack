// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Startup.cs" company="">
//   
// </copyright>
// <summary>
//   The startup.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi
{
    using System;
    using System.Text.Encodings.Web;
    using System.Text.Unicode;

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

    using Swashbuckle.AspNetCore.Swagger;

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
                    .AddJsonFile("appsettings.json", true, true)
                    .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", true)
                    .AddJsonFile("MyJson.json", false).AddEnvironmentVariables();
                config = builder.Build();
            }

            this.Configuration = config;
            this.Environment = environment;
        }

        /// <summary>
        ///     Gets the configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        ///     Gets the environment.
        /// </summary>
        public IHostingEnvironment Environment { get; }

        /// <summary>
        /// The configure.
        ///     This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">
        /// The app.
        /// </param>
        /// <param name="env">
        /// The env.
        /// </param>
        /// <param name="loggerFactory">
        /// The logger factory.
        /// </param>
        /// <param name="serviceProvider">
        /// The service provider.
        /// </param>
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IServiceProvider serviceProvider)
        {
            app.UseCors();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
            app.UseRequestLocalization();
            if (this.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "1cSupport V1"); });

                SeedData.EnsurePopulated(app);
            }
            else
            {
                app.UseHsts();
            }
        }

        /// <summary>
        /// The configure services.
        ///     This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">
        /// The services.
        /// </param>
        /// <returns>
        /// The <see cref="IServiceProvider"/>.
        /// </returns>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            Console.WriteLine(this.Configuration["Connection:String"]);

            var tokenParams = TokenValidationParametersBuilder.GetTokenValidationParameters(this.Configuration);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                config => { config.TokenValidationParameters = tokenParams; });

            services.AddDbContext<EFContext>(
                options =>
                    {
                        //if (this.Environment.IsDevelopment()) options.UseInMemoryDatabase("TestDb");
                        //else
                            options.UseSqlServer(
                                this.Configuration["Connection:String"],
                                b => b.MigrationsAssembly("WebApi"));
                    });

            services.AddSingleton(tokenParams);

            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.Configure<WebEncoderOptions>(
                options => options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All));

            Console.WriteLine($"{this.Configuration["Email:Server"]}:{this.Configuration["Email:Port"]}");

            services.AddMailKit(
                optionBuilder =>
                    {
                        optionBuilder.UseMailKit(
                            new MailKitOptions
                                {
                                    // get options from sercets.json
                                    Server = this.Configuration["Email:Server"],
                                    Port = Convert.ToInt32(this.Configuration["Email:Port"]),
                                    SenderName = this.Configuration["Email:SenderName"],
                                    SenderEmail = this.Configuration["Email:SenderEmail"],

                                    // can be optional with no authentication
                                    Account = this.Configuration["Email:Account"],
                                    Password = this.Configuration["Email:Password"],

                                    // enable ssl or tls
                                    Security = true
                                });
                    });

            services.AddSwaggerGen(
                c =>
                    {
                        c.SwaggerDoc("v1", new Info { Title = "1cSupport", Version = "v1" });
                        c.AddSecurityDefinition(
                            "Bearer",
                            new ApiKeyScheme
                                {
                                    Description =
                                        "JWT Authorization header using the Bearer scheme. Example - Authorization: Bearer {token}",
                                    Name = "Authorization",
                                    In = "header",
                                    Type = "apiKey"
                                });
                    });

            return services.BuildServiceProvider();
        }
    }
}