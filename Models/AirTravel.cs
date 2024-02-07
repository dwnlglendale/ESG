using System.ComponentModel.DataAnnotations;

namespace CarbonFootprint1.Models
{
  
    public class AirTravel
    {
        [Key]
        public int ID { get; set; }
        public int? DomesticTravelCount { get; set; }
        public int? InternationalTravelCount { get; set; }
        public int? DomesticTravelCost { get; set; }
        public int? InternationalTravelCost { get; set; }
        public int? DepartureAirport { get; set; }
        public int? ArrivalAirport { get; set; }
        public int? DomDistanceTravelled { get; set; }
        public int? IntDistanceTravelled { get; set; }
        public DateTime ReportMonth { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public FormStatus Status { get; set; } = FormStatus.Pending;
    }
}
