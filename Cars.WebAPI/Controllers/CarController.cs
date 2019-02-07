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
    public class CarController : ControllerBase
    {
        //https://localhost:5001/v1/Car?colors=5&colors=2
        /// <summary>
        /// Get Cars
        /// </summary> 
        /// <response code="200">Ok</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IList<CarModel>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IList<CarModel>>> GetCars(string brand, string model, short? year, EnumColor[] colors, string[] optionals)
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

            return await Task.FromResult<ActionResult>(this.Ok(cars)); ;
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
        [ProducesResponseType(typeof(CarModel), 200)]
        [ProducesResponseType(typeof(ErrorModel), 400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<CarModel>> GetCar(string id)
        {
            Guid identifier = Guid.Empty;
            if (!Guid.TryParse(id, out identifier))
                return await Task.FromResult<ActionResult>(this.BadRequest(new ErrorModel(1, "Id", "Invalid ID!").ToList()));

            var car = this.SelectCar(identifier);

            if (car == null) return await Task.FromResult<ActionResult>(this.NotFound());
            else return await Task.FromResult<ActionResult>(this.Ok(car)); ;
        }

        /// <summary>
        /// Create Car
        /// </summary> 
        /// <response code="201">Created</response>
        /// <response code="400">Bad Request</response>
        /// <response code="422">Unprocessable Entity</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(CarModel), 201)]
        [ProducesResponseType(typeof(ErrorModel), 400)]
        [ProducesResponseType(typeof(string), 422)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<CarModel>> CreateCar([FromBody] CarAddModel car)
        {
            if (car == null)
                return await Task.FromResult<ActionResult>(this.BadRequest(new ErrorModel(1, "Id", "The car can not be null!").ToList()));

            if (car.Year < 1885)
                return await Task.FromResult<ActionResult>(this.UnprocessableEntity("Vehicle year is invalid!"));

            if (!Enum.IsDefined(typeof(EnumColor), car.Color))
                return await Task.FromResult<ActionResult>(this.UnprocessableEntity("Color of the vehicle is invalid!"));

            if(CarRepository.Cars.Count() > 30) return await Task.FromResult<ActionResult>(this.UnprocessableEntity("It is not possible to add more vehicles, please delete the available vehicles!"));

            var result = this.InsertCar(car);

            if (result == null) return await Task.FromResult<ActionResult>(this.UnprocessableEntity("This car has already been registered!"));
            else return await Task.FromResult<ActionResult>(this.Created(result.Id.ToString(), result));
        }

        /// <summary>
        /// Update Car
        /// </summary> 
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CarModel), 200)]
        [ProducesResponseType(typeof(ErrorModel), 400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<CarModel>> UpdateCar(string id, [FromBody] CarAlterModel car)
        {
            Guid identifier = Guid.Empty;
            if (!Guid.TryParse(id, out identifier))
                return await Task.FromResult<ActionResult>(this.BadRequest(new ErrorModel(1, "Id", "Invalid ID!").ToList()));

            if (car == null)
                return await Task.FromResult<ActionResult>(this.BadRequest(new ErrorModel(1, "Id", "The car can not be null!").ToList()));

            var selectedCar = this.SelectCar(identifier);
            if (selectedCar == null) return await Task.FromResult<ActionResult>(this.NotFound());
            else this.AlterCar(selectedCar, car);

            selectedCar = this.SelectCar(identifier);

            return await Task.FromResult<ActionResult>(this.Ok(selectedCar));
        }

        /// <summary>
        /// Remove Car
        /// </summary> 
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        /// <response code="422">Unprocessable Entity</response>
        /// <response code="500">Internal Server Error</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(ErrorModel), 400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<string>> RemoveCar(string id)
        {
            Guid identifier = Guid.Empty;
            if (!Guid.TryParse(id, out identifier))
                return await Task.FromResult<ActionResult>(this.BadRequest(new ErrorModel(1, "Id", "Invalid ID!").ToList()));

            var selectedCar = this.SelectCar(identifier);
            if (selectedCar == null) return await Task.FromResult<ActionResult>(this.NotFound());

            var removed = CarRepository.Cars.Remove(selectedCar);
            if (removed == false) return await Task.FromResult<ActionResult>(this.UnprocessableEntity());
            else return await Task.FromResult<ActionResult>(this.Ok("The car was successfully removed."));
        }


        private void AlterCar(CarModel selectedCar, CarAlterModel car)
        {
            selectedCar.Color = car.Color;
            selectedCar.Optionals = car.Optionals;
        }

        private CarModel SelectCar(Guid identifier)
        {
            return CarRepository.Cars.FirstOrDefault(x => x.Id == identifier);
        }

        private CarModel InsertCar(CarAddModel car)
        {
            if (CarRepository.Cars.Any(x =>
                x.Brand.Equals(car.Brand, StringComparison.OrdinalIgnoreCase) &&
                x.Model.Equals(car.Model, StringComparison.OrdinalIgnoreCase) &&
                x.Year == car.Year))
                return null;

            var entity = new CarModel(car);
            CarRepository.Cars.Add(entity);
            return entity;
        }
    }
}
