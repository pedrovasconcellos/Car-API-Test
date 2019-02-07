using Cars.WebAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cars.WebAPI.Controllers
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    public class OptionalCarController : ControllerBase
    {
        /// <summary>
        /// Get Optionals
        /// </summary> 
        /// <response code="200">Ok</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IList<string>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IList<string>>> GetOptionals()
        {
            IList<string> result = new List<string>();
            foreach (var car in CarRepository.Cars)
            {
                foreach (var optional in car.Optionals)
                {
                    if (!result.Any(o => o.Equals(optional, StringComparison.OrdinalIgnoreCase)))
                        result.Add(optional);
                }

            }

            Request.HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "X-Total-Count");
            Request.HttpContext.Response.Headers.Add("X-Total-Count", result?.Count().ToString());

            if (result == null || result.Count == 0) return await Task.FromResult<ActionResult>(this.Ok(result));
            else return await Task.FromResult<ActionResult>(this.Ok(result));
        }
    }
}
