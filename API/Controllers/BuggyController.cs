using API.Errors;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

///updated BuggyController USES ApiResponse.cs class to return a SPECIFIC HTTP Error message
namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly StoreContext _context;

        public BuggyController(StoreContext context)
        {
            _context = context;
        }

        // this method returns a NOT-FOUND ERROR
        [HttpGet("notfound")]
        public ActionResult GetNotFoundRequest() 
        {
            int testNotFoundId = 42;
            var thing = _context.Products.Find(testNotFoundId);

            if (thing == null)
            {
                int statusCode = 404;
                ApiResponse errorMessage = new ApiResponse(statusCode);
                return NotFound(errorMessage);
            }

            return Ok();
        }

        // this method returns a SERVER ERROR
        // the ApiException.cs class will returns a 
        // Internal SERVER ERROR message when the HTTP request
        // ROUTED to ErrorController.cs class
        [HttpGet("servererror")]
        public ActionResult GetServerError() 
        {
            int testServerErrorId = 42;

            var thing = _context.Products.Find(testServerErrorId);

            var thingToReturn = thing.ToString();

            return Ok();
        }

        // this method returns a BAD-REQUEST ERROR
        [HttpGet("badrequest")]
        public ActionResult GetBadRequest() 
        {
            int statusCode = 400;
            ApiResponse errorMessge = new ApiResponse(statusCode);
            return BadRequest(errorMessge);
        }

        // this method returns a VALIDATION ERROR
        // using ExceptionMiddleWare.cs class
        [HttpGet("badrequest/{id}")]
        public ActionResult GetBadRequest(int id) 
        {
            int testNotFoundId = 42;
            var thing = _context.Products.Find(testNotFoundId);

            if (thing == null)
            {
                int statusCode = 404;
                ApiResponse errorMessage = new ApiResponse(statusCode);
                return NotFound(errorMessage);
            }
            return Ok(thing);
        }
    }
}



// **************************************************************


/// HOW BuggyController Display Different TYPES of ERRORS
// namespace API.Controllers
// {
//     public class BuggyController : BaseApiController
//     {
//         private readonly StoreContext _context;

//         public BuggyController(StoreContext context)
//         {
//             _context = context;
//         }

//         [HttpGet("notfound")]
//         public ActionResult GetNotFoundRequest() 
//         {
//             int testNotFoundId = 4000;
//             var thing = _context.Products.Find(testNotFoundId);

//             if (thing == null)
//             {
//                 return NotFound();
//             }

//             return Ok();
//         }

//         [HttpGet("servererror")]
//         public ActionResult GetServerError() 
//         {
//             int testServerErrorWithNotFoundId = 4000;

//             var thing = _context.Products.Find(testServerErrorWithNotFoundId);

//             var thingToReturn = thing.ToString(); // thing == NULL & tostring( ) => Excepton => Error message

//             return Ok();
//         }

//         [HttpGet("badrequest")]
//         public ActionResult GetBadRequest() 
//         {
//             return BadRequest();
//         }

//         //this method returns a VALIDATION ERROR
//         [HttpGet("badrequest/{id}")]
//         public ActionResult GetBadRequest(int id) 
//         {
//             return Ok();
//         }
//     }
// }











