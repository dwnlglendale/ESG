using System.ComponentModel.DataAnnotations;


namespace CarbonFootprint1.Models
{

    public enum FormStatus
    {
        Pending,
        Approved,
        Returned,
        Draft
    }


    public class BranchDetails
    {
        //Branch Details

        [Key]
        public int CarbornFootprint { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ReportMonth { get; set; } 
        public string? BranchCode { get; set; }
        public string? BranchName { get; set; }
        public double? BranchSize { get; set; }
        public int? PermanentStaffNumber { get; set; }
        public int? NonPermanentStaffNumber { get; set; }
        public int? TotalStaffNumber { get; set; }




        //Electricty

        public double? ElectricityConsumed { get; set; }
        public double? ElectricityAmount { get; set; }
        public string? AlternativeSourceOfEnergy { get; set; }
        public string? MeterType { get; set; }
        public double? ElectricityCO2Emission { get; set; }
        public double? ElectricityCO2KGS { get; set; }
        public double? StaffElectricityEmission { get; set; }
        public double? StaffElectricityConsumption { get; set; }

        //Generator Diesel Consumption
        public double? AmountPaidForDieselGenerator { get; set; }
        public double? QuantityOfDieselPurchased { get; set; }
        public double? QuantityOfDieselConsumed { get; set; }
        public double? QuantityOfDieselLeft { get; set; }
        public double? TotalRuningGeneratorHours { get; set; }
        public double? DieselGeneratorCO2Emission { get; set; }
        public double? DieselGeneratorCO2KGS { get; set; }

        public string? DocumentsPath { get; set; }


        //Water Consumption
        public double? QuantityOfDrinkableWaterConsumed { get; set; }
        public double? QuantityOfRegularWaterConsumed { get; set; }
        public double? CostOfRegularWaterConsumed { get; set; }
        public double? CostOfDrinkableWaterConsumed { get; set; }
        public double? StaffWaterConsumption { get; set; }

        //Paper consumption
        public int? NumberOfPaperUsed { get; set; }
        public double? CostOfPaperUsed { get; set; }
        public double? StaffPaperConsumption { get; set; }

        //Waste Consumption
        public string? MeansOfDisposal { get; set; }
        public double? CostOfDisposal { get; set; }
        public double? QuantityOfDisposal { get; set; }
        public double? StaffWasteAccumulation { get; set; }
      

        //Form Status 
        public FormStatus Status { get; set; }
        public string? Comment { get; set; }


        public BranchDetails()
        {
            Status = FormStatus.Pending;
        }

    }
}
