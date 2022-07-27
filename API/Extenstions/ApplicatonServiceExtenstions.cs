
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
                   });
            return services;
        }
    }
}
