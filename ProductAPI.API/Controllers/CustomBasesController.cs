using Microsoft.AspNetCore.Mvc;
using ProductAPI.Core.DTOs;

namespace ProductAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBasesController : ControllerBase
    {
        [NonAction]
        public IActionResult CreateActionResult<T>(GlobalResultDto<T> response)
        {
            if (response.StatusCode == 204)
            {
                return new ObjectResult(null)
                {
                    StatusCode = response.StatusCode
                };
            }
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
    }
}
