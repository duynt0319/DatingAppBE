
using API.Data;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using API.MiddleWare;
using API.Services;
using API.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
                                  });
            });



            // Add services to the container.
            builder.Services.AddControllers();

            //cấu hình kết nối db và cấu hình để fetchAPI
            builder.Services.AddApplicationServices(builder.Configuration);

            //cấu hình jwt, authorize và authentication
            builder.Services.AddIdentityServices(builder.Configuration);


            var app = builder.Build();

            //cors
            app.UseCors(MyAllowSpecificOrigins);

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

            //cấu hình để xử lý exception
            app.UseMiddleware<ExceptionMiddleware>();

            //cấu hình để bên front end có thể gọi được api
            app.UseCors(builder => builder
                                        .AllowAnyHeader()
                                        .AllowAnyMethod()
                                        .AllowCredentials()
                                        .WithOrigins("https://localhost:4200"));

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.MapControllers();
            app.MapHub<PresenceHub>("hubs/presence");
            app.MapHub<MessageHub>("hubs/message");
            app.MapFallbackToController("Index", "Fallback");


            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<DataContext>();
                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
                await context.Database.MigrateAsync();
                await Seed.ClearConnection(context);
                await Seed.SeedUsers(userManager, roleManager);
            }
            catch (Exception ex)
            {
                var logger = services.GetService<ILogger<Program>>();
                logger.LogError(ex, "An error occured during migration");
            }

            app.Run();
        }
    }
}