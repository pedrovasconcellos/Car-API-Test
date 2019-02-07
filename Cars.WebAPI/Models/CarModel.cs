using System;

namespace Cars.WebAPI.Models
{
    public class CarModel
    {
        public CarModel() { }

        public CarModel(CarAddModel car)
        {
            this.Id = Guid.NewGuid();
            this.Brand = car.Brand;
            this.Model = car.Model;
            this.Year = car.Year;
            this.Color = car.Color;
            this.Optionals = car.Optionals;
        }

        public Guid Id { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public short Year { get; set; }

        public EnumColor Color { get; set; }

        public string[] Optionals { get; set; }
    }
}
