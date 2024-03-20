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
            ////cấu hình để kết nối với MQLDB
            //services.AddDbContext<DataContext>(opt =>
            //{
            //    opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            //});

            //cấu hình để kết nối với PostGres DB
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });

            //cấu hình để có thể gọi các endpoint
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("CorsPolicy",
            //        builder => builder
            //            .WithOrigins("http://localhost:8080") // Cho phép yêu cầu từ domain này
            //            .AllowAnyMethod()
            //            .AllowAnyHeader()
            //            .AllowCredentials()); // Nếu bạn sử dụng cookie hoặc credential, bạn cần cung cấp AllowCredentials
            //});
            //add jwt
            services.AddScoped<ITokenService, TokenService>();

            //add cau hinh repository
            //services.AddScoped<IUserRepository, UserRepository>();
            //services.AddScoped<ILikesRepository, LikeRepository>();
            //add cau hinh cho message
            //services.AddScoped<IMessageRepository, MessageRepository>();

            //add cau hinh mapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //add cau hinh Cloudinary
            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));
            services.AddScoped<IPhotoService, PhotoService>();
            //add cau hinh userlogActivity
            services.AddScoped<LogUserActivity>();
            
            //add signalR
            services.AddSignalR();
            services.AddSingleton<PresenceTracker>();
            //add cau hinh unitofwork
            services.AddScoped<IUnitOfWord, UnitOfWork>();


            return services;
        }
    }
}
