﻿using Cars.WebAPI.Extensions;
using Cars.WebAPI.Help;
using Cars.WebAPI.Models;
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
    public class CarXmlController : ControllerBase
    {
        /// <summary>
        /// Get Cars
        /// </summary> 
        /// <response code="200">Ok</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IList<string>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IList<string>>> GetCars(string brand, string model, short? year, EnumColor[] colors, string[] optionals)
        {
            IEnumerable<CarModel> cars = CarRepository.Cars;

            if (!string.IsNullOrEmpty(brand))
                cars = cars.Where(x => x.Brand.Contains(brand, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(model))
                cars = cars.Where(x => x.Model.Contains(model, StringComparison.OrdinalIgnoreCase));

            if (year != null && year >= 0)
                cars = cars.Where(x => x.Year == year);

            if (colors != null && colors.Count() > 0)
                cars = cars.Where(x => colors.Any(y => y == x.Color));

            if (optionals != null && optionals.Count() > 0 && !string.IsNullOrEmpty(optionals.FirstOrDefault()))
                cars = cars.Where(car => optionals.Any(v => car.Optionals.Any(o => o.Contains(v, StringComparison.OrdinalIgnoreCase))));

            Request.HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "X-Total-Count");
            Request.HttpContext.Response.Headers.Add("X-Total-Count", cars?.Count().ToString());

            return await Task.FromResult<ActionResult>(this.Ok(XmlExtension.ObjectToXml(cars.ToList())));
        }

        /// <summary>
        /// Get Car
        /// </summary> 
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(ErrorModel), 400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<string>> GetCar(string id)
        {
            Guid identifier = Guid.Empty;
            if (!Guid.TryParse(id, out identifier))
                return await Task.FromResult<ActionResult>(this.BadRequest(new ErrorModel(1, "Id", "Invalid ID!").ToList()));

            var car = this.SelectCar(identifier);

            if (car == null) return await Task.FromResult<ActionResult>(this.NotFound());
            else return await Task.FromResult<ActionResult>(this.Ok(XmlExtension.ObjectToXml(car)));
        }

        private CarModel SelectCar(Guid identifier)
        {
            return CarRepository.Cars.FirstOrDefault(x => x.Id == identifier);
        }
    }
}
