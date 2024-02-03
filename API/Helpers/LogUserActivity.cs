using API.Extensions;
using API.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var reultContext = await next();

            if (!reultContext.HttpContext.User.Identity.IsAuthenticated) return;

            var userId = reultContext.HttpContext.User.GetUserId();

            var repo = reultContext.HttpContext.RequestServices.GetRequiredService<IUserRepository>();

            var user = await repo.GetUserByIdAsync(int.Parse(userId));
            user.LastActive = DateTime.UtcNow;
            await repo.SaveAllAsyn();
        }
    }
}
