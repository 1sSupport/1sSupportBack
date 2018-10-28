using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.WebEncoders;
using Microsoft.IdentityModel.Tokens;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using WebApi.EF.Models;
using WebApi.Infrastructer;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IHostingEnvironment Environment { get; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var tokenParam = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                //ValidateLifetime = true,
                ValidIssuer = $"{Configuration["JWT:issuer"]}",
                ValidAudience = $"{Configuration["JWT:audience"]}",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:key"]))
            };
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(config =>
                {
                    config.TokenValidationParameters = tokenParam;
                });

            services.AddDbContext<EFContext>(options => { options.UseInMemoryDatabase("MyDatabase"); });
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.Configure<WebEncoderOptions>(options => options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All));
            if (!Environment.IsDevelopment())
            {
                return services.BuildServiceProvider();
            }

            Console.WriteLine($"{Configuration["Email:Server"]}:{Configuration["Email:Port"]}");
            services.AddMailKit(optionBuilder =>
            {
                optionBuilder.UseMailKit(new MailKitOptions()
                {
                    //get options from sercets.json
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            app.UseCors();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
            app.UseRequestLocalization();
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            SeedData.EnsurePopulated(app);
        }
    }
}