using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Extenstions;
using API.Interfaces;
using API.Middelware;
using API.Services;
using API.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySqlConnector;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationServices(_config);
            // services.AddTransient<MySqlConnection>(_ => new MySqlConnection(_config.GetConnectionString("DefaultConnection")));
            services.AddControllers();
            services.AddSwaggerGen(c =>
 {
     // configure SwaggerDoc and others

     // add JWT Authentication
     var securityScheme = new OpenApiSecurityScheme
     {
         Name = "JWT Authentication",
         Description = "Enter JWT Bearer token **_only_**",
         In = ParameterLocation.Header,
         Type = SecuritySchemeType.Http,
         Scheme = "bearer", // must be lower case
         BearerFormat = "JWT",
         Reference = new OpenApiReference
         {
             Id = JwtBearerDefaults.AuthenticationScheme,
             Type = ReferenceType.SecurityScheme
         }
     };
     c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
     c.AddSecurityRequirement(new OpenApiSecurityRequirement
     {
        {securityScheme, new string[] { }}
     });

     // add Basic Authentication
     var basicSecurityScheme = new OpenApiSecurityScheme
     {
         Type = SecuritySchemeType.Http,
         Scheme = "basic",
         Reference = new OpenApiReference { Id = "BasicAuth", Type = ReferenceType.SecurityScheme }
     };
     c.AddSecurityDefinition(basicSecurityScheme.Reference.Id, basicSecurityScheme);
     c.AddSecurityRequirement(new OpenApiSecurityRequirement
     {
        {basicSecurityScheme, new string[] { }}
     });
 });
            services.AddCors();
            services.AddIdentityService(_config);
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BasicAuth v1"));
            }
            app.UseMiddleware<ExeptionMiddelware>();
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:4200"));
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();              
                endpoints.MapHub<PresenceHub>("hubs/presence");
                endpoints.MapHub<MessageHub>("hubs/message");
                // endpoints.MapFallbackToController("Index", "Fallback");
            });
        }
    }
}
