using Libri.BAL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Libri.API.Helpers
{
    [AttributeUsage((AttributeTargets.Method))]
    public class InvalidateCache : Attribute, IAsyncActionFilter
    {
        private readonly string _pattern;
        public InvalidateCache(string pattern)
        {
            _pattern = pattern;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            if (resultContext.Exception == null || resultContext.ExceptionHandled)
            {
                var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
                await cacheService.RemoveCacheByPattern(_pattern);
            } 
        }
    }
}