using API.Data;
using API.Helpers;
using API.Implement;
using API.Interfaces;
using API.Services;
using API.SignalR;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            //cấu hình để kết nối với DB
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            //cấu hình để có thể gọi các endpoint
            services.AddCors();
            //add jwt
            services.AddScoped<ITokenService, TokenService>();
            //add cau hin repository
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILikesRepository,LikeRepository>();
            //add cau hinh mapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //add cau hinh Cloudinary
            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));
            services.AddScoped<IPhotoService, PhotoService>();
            //add cau hinh userlogActivity
            services.AddScoped<LogUserActivity>();
            //add cau hinh cho message
            services.AddScoped<IMessageRepository, MessageRepository>();
            //add signalR
            services.AddSignalR();
            services.AddSingleton<PresenceTracker>();


            return services;
        }
    }
}
