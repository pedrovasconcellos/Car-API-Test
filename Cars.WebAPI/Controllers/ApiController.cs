using Cars.WebAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cars.WebAPI.Controllers
{
    [ApiVersionNeutral]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        /// <summary>
        /// Reset API
        /// </summary> 
        /// <response code="200">Ok</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost()]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<string>> ResetAPI()
        {
            CarRepository.SetDefault();

            return await Task.FromResult<ActionResult>(this.Ok("The WebAPI has been reset."));
        }
    }
}
