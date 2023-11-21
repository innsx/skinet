using API.Errors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("errors/{statusCode}")]
    // adding this configuration will NOW IGNORE BY API.Controller base class to NOT looking for ANY MATCHING ENDPOINTS
    // Instead API.Contoller will use SWAGGER to ROUTE and RESET all ENDPOINTS
    // WHEN RUNING THIS ErrorControll.cs class
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : BaseApiController
    {
        public IActionResult Error(int statusCode)
        {
            return new ObjectResult(new ApiResponse(statusCode));
        }
    }
}