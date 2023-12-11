
using API.Data;
using API.Extensions;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            //cấu hình kết nối db và cấu hình để fetchAPI
            builder.Services.AddApplicationServices(builder.Configuration);
            
            //cấu hình jwt, authorize và authentication
            builder.Services.AddIdentityServices(builder.Configuration);


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");

                    // Thêm nút Authorize trong Swagger UI
                    c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
                    c.OAuthAppName("Your API - Swagger UI");
                    c.OAuthAdditionalQueryStringParams(new Dictionary<string, string>
                    {
                        { "access_token", "your-jwt-token" }
                    });
                });
            }

            //cấu hình để bên front end có thể gọi được api
            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}