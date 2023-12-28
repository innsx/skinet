using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace API.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveInSeconds;
        public CachedAttribute(int timeToLiveInSeconds)
        {
            _timeToLiveInSeconds = timeToLiveInSeconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

            string cacheKey = GenerateACacheKeyFromHttpRequest(context.HttpContext.Request);

            string cacheResponse = await cacheService.GetCachedResponseAsync(cacheKey);

            // if there is a CacheResponse, then create contentResult
            if (!string.IsNullOrEmpty(cacheResponse))
            {
                var contentResult = new ContentResult
                {
                    Content = cacheResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };

                // bind the contentResult to the content.Result
                context.Result = contentResult;

                // returns the content back to the Client
                return;
            }

            // Else there is NO cacheResponse, 
            // then move to controller class and have the Controller class
            // execute it's HTTPGet request from the Database
            var executedContext = await next(); 

            // Check if there are responses coming from DB,
            // then cache the responses in Redis Server
            // and the NEXT time this information is available in MEMORY
            // to retrieve instead of having the API going to the DB and get
            // the information there
            if (executedContext.Result is OkObjectResult okObjectResult)
            {
                await cacheService.SetCacheResponseAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLiveInSeconds));
            }
        }

        private string GenerateACacheKeyFromHttpRequest(HttpRequest httpRequest)
        {
            StringBuilder keyStringBuilder = new StringBuilder();

            keyStringBuilder.Append($"{httpRequest.Path}");

            foreach (var (key, value) in httpRequest.Query.OrderBy(x => x.Key))
            {
                keyStringBuilder.Append($"|{key}-{value}");
            }

            string keyString = keyStringBuilder.ToString();

            return keyString;
        }
    }
}
