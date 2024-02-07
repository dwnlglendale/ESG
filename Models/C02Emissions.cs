

namespace CarbonFootprint1.Models
{
    public class C02Emissions
    {
        public double? DieselEmission { get; set; }
        public double PetrolEmission { get; set; }
        public double ShortHaulEmission { get; set; }
        public double LongHaulEmission { get; set; }
        public double HydroElectricityEmission { get; set; }
        public double GasElectricityEmission { get; set; }
        public double TotalElectricityEmission { get; set; }


    }
}
