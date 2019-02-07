using Cars.WebAPI.Extensions;
using Cars.WebAPI.Help;
using Cars.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cars.WebAPI.Controllers
{
    [ApiVersionNeutral]
    [Route("[controller]")]
    public class CarColorController : ControllerBase
    {        
        /// <summary>
        /// Get Colors
        /// </summary> 
        /// <response code="200">Ok</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [AllowAnonymous]
        public async Task<ActionResult<IList<EnumeratorModel>>> GetColors()
        {
            var result = EnumExtension.EnumToDictonary<EnumColor>();

            Request.HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "X-Total-Count");
            Request.HttpContext.Response.Headers.Add("X-Total-Count", result?.Count().ToString());

            if (result == null || result.Count == 0) return await Task.FromResult<ActionResult>(this.Ok(result));
            else return await Task.FromResult<ActionResult>(this.Ok(result));
        }
    }
}
