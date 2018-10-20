using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using System;
using System.Text;

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
        public void ConfigureServices(IServiceCollection services)
        {
            var tokenParam = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = $"{Configuration["JWT:issuer"]}",
                ValidAudience = $"{Configuration["JWT:audience"]}",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:key"]))
            };
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(config =>
                {
                    config.TokenValidationParameters = tokenParam;
                });

            if (Environment.IsDevelopment())
            {
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
            }

            // services.AddDbContext<EFContext>(options => { options.UseInMemoryDatabase(); });
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
        }
    }
}