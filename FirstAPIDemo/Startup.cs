using FirstAPIDemo.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FirstAPIDemo
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(config => {
                config.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "SQ009 First API Demo",
                    Version = "v1",
                    Description = "Simple API Sample"
                });
            });

            services.AddTransient<IJwtService, JwtService>();

            services.AddAuthentication(option => {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option => {

                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:SecurityKey"])),
                    ValidateIssuerSigningKey = true
                };

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // middlewares

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder =>
                {
                    builder.Run( async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        var contentFeatures = context.Features.Get<IExceptionHandlerFeature>();
                        context.Response.Headers.Add("Application-Error", contentFeatures.Error.Message);
                        context.Response.Headers.Add("Application-Control-Expose-Header", "Application-Error");
                        context.Response.Headers.Add("Application-Control-Allow-Origin", "*");
                        //await context.Response.wr();
                        //context.Response.
                    });
                });
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "SQ009 First API - v1"));
        }
    }
}
