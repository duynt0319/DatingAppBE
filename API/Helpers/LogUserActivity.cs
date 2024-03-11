using API.Extensions;
using API.Interfaces;
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

            var uow = reultContext.HttpContext.RequestServices.GetRequiredService<IUnitOfWord>();

            var user = await uow.UserRepository.GetUserByIdAsync(userId);
            user.LastActive = DateTime.UtcNow;
            await uow.Complete();
        }
    }
}
