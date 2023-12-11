using API.Data;
using API.Interfaces;
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

            return services;
        }
    }
}
