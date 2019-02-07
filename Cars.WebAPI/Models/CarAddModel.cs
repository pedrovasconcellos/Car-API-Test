
namespace Cars.WebAPI.Models
{
    public class CarAddModel
    {
        public string Brand { get; set; }

        public string Model { get; set; }

        public short Year { get; set; }

        public EnumColor Color { get; set; }

        public string[] Optionals { get; set; }
    }
}
