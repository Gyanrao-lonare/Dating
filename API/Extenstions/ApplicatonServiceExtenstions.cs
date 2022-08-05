
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using API.Interfaces;
using API.Services;
using API.Data;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using API.Helpers;
using API.SignalR;

namespace API.Extenstions
{
    public static class ApplicatonServiceExtenstions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            
            services.AddSingleton<PresenceTracker>();
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<ILikesRepository, LikesRepository>();
            
            services.AddScoped<IFriendRequestRepository, FriendRequestRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IStatusRepository, StatusRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddAutoMapper(typeof(AutoMaperProfile).Assembly);
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<LogUserActivity>();
            services.AddDbContext<DataContext>(options =>
                   {
                    //    options.UseMySQL(config.GetConnectionString("DefaultConnection1"));
                    //    options.UseSqlite(config.GetConnectionString("DefaultConnection"));
                        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                string connStr;

                // Depending on if in development or production, use either Heroku-provided
                // connection string, or development connection string from env var.
                 if (env == "Development")
                {
                     var connectionString = config.GetConnectionString("DefaultConnection1");
                     options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                    // Use connection string from file.
                    // connStr = config.GetConnectionString("DefaultConnection");
                }
                else
                {
                    // Use connection string provided at runtime by Heroku.
                    var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

                    // Parse connection URL to connection string for Npgsql
                    connUrl = connUrl.Replace("postgres://", string.Empty);
                    var pgUserPass = connUrl.Split("@")[0];
                    var pgHostPortDb = connUrl.Split("@")[1];
                    var pgHostPort = pgHostPortDb.Split("/")[0];
                    var pgDb = pgHostPortDb.Split("/")[1];
                    var pgUser = pgUserPass.Split(":")[0];
                    var pgPass = pgUserPass.Split(":")[1];
                    var pgHost = pgHostPort.Split(":")[0];
                    var pgPort = pgHostPort.Split(":")[1];

                    connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb}; SSL Mode=Require; Trust Server Certificate=true"; 
                    options.UseNpgsql(connStr);
                }

                // Whether the connection string came from the local development configuration file
                // or from the environment variable from Heroku, use it to set up your DbContext.
             
                       
        });

            return services;
        }
    }
}