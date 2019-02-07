using Cars.WebAPI.Models;
using System;
using System.Collections.Generic;

namespace Cars.WebAPI.Repository
{
    internal static class CarRepository
    {
        internal static IList<CarModel> Cars { get; set; }

        internal static void SetDefault()
        {
            Cars = new List<CarModel>();

            var ferrari = new CarModel()
            {
                Id = Guid.Parse("7f430a38-a6b2-4a8f-96d5-801725dfdfc8"),
                Brand = "Ferrari",
                Model = "F50",
                Color = EnumColor.Red,
                Year = 2018,
                Optionals = new string[] { "Air conditioning", "Cellphone alarm", "GPS" }
            };

            var lamborghini = new CarModel()
            {
                Id = Guid.Parse("b413cfc0-f53a-4765-9430-3912efcd79cb"),
                Brand = "Lamborghini",
                Model = "Gallardo",
                Year = 2019,
                Color = EnumColor.Yellow,
                Optionals = new string[] { "Air conditioning", "GPS", "Multi media" }
            };

            var porsche = new CarModel()
            {
                Id = Guid.Parse("b413cfc0-f53a-4765-9430-3912efcd79cb"),
                Brand = "Porsche",
                Model = "Carrera",
                Year = 2015,
                Color = EnumColor.Purple,
                Optionals = new string[] { "Air conditioning", "Multi media" }
            };

            var bmw = new CarModel()
            {
                Id = Guid.Parse("a714554f-f363-42f1-b41a-81ee85186660"),
                Brand = "BMW",
                Model = "320i",
                Year = 2017,
                Color = EnumColor.White,
                Optionals = new string[] { "Air conditioning", "Multi media" }
            };

            var mercedes = new CarModel()
            {
                Id = Guid.Parse("a714554f-f363-42f1-b41a-81ee85186660"),
                Brand = "Mercedes-Benz",
                Model = "c200",
                Year = 2019,
                Color = EnumColor.Silver,
                Optionals = new string[] { "Air conditioning", "Alarm" }
            };

            CarRepository.Cars.Add(ferrari);
            CarRepository.Cars.Add(lamborghini);
            CarRepository.Cars.Add(porsche);
            CarRepository.Cars.Add(bmw);
            CarRepository.Cars.Add(mercedes);
        }
    }
}
