using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Repository;
using API.Repository.IRepository;
using API.Services;
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
            //add cau hinh mapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //add cau hinh Cloudinary
            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));
            services.AddScoped<IPhotoService, PhotoService>();

            return services;
        }
    }
}
