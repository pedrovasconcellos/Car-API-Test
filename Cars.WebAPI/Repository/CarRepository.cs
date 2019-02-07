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

            var car1 = new CarModel()
            {
                Id = Guid.Parse("7f430a38-a6b2-4a8f-96d5-801725dfdfc8"),
                Brand = "Ferrari",
                Model = "F50",
                Color = EnumColor.Red,
                Year = 2018,
                Optionals = new string[] { "Air conditioning", "GPS" }
            };
            var car2 = new CarModel()
            {
                Id = Guid.Parse("b413cfc0-f53a-4765-9430-3912efcd79cb"),
                Brand = "Lamborghini",
                Model = "Gallardo",
                Year = 2019,
                Color = EnumColor.Yellow,
                Optionals = new string[] { "Air conditioning", "Multi media" }
            };
            
            CarRepository.Cars.Add(car1);
            CarRepository.Cars.Add(car2);
        }
    }
}
