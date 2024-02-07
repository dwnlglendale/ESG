using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarbonFootprint1.Migrations
{
    public partial class Footprintsetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FootprintTable",
                columns: table => new
                {
                    CarbornFootprint = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchCode = table.Column<int>(type: "int", nullable: true),
                    BranchName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BranchSize = table.Column<double>(type: "float", nullable: true),
                    PermanentStaffNumber = table.Column<int>(type: "int", nullable: true),
                    NonPermanentStaffNumber = table.Column<int>(type: "int", nullable: true),
                    ElectricityConsumed = table.Column<double>(type: "float", nullable: true),
                    ElectricityAmount = table.Column<double>(type: "float", nullable: true),
                    AlternativeSourceOfEnergy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AmountPaidForDieselGenerator = table.Column<int>(type: "int", nullable: true),
                    QuantityOfDieselPurchased = table.Column<int>(type: "int", nullable: true),
                    QuantityOfDieselConsumed = table.Column<int>(type: "int", nullable: true),
                    QuantityOfDieselLeft = table.Column<int>(type: "int", nullable: true),
                    TotalRuningGeneratorHours = table.Column<int>(type: "int", nullable: true),
                    NumberOfVehicles = table.Column<double>(type: "float", nullable: true),
                    VehicleBranchShare = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleBranchList = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleUsers = table.Column<int>(type: "int", nullable: true),
                    HoursUsedByDifferentBranch = table.Column<int>(type: "int", nullable: true),
                    FuelType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DieselCostPerLitre = table.Column<double>(type: "float", nullable: true),
                    PetrolCostPerLitre = table.Column<double>(type: "float", nullable: true),
                    AmountPaidForPetrol = table.Column<double>(type: "float", nullable: true),
                    AmountPaidForDiesel = table.Column<double>(type: "float", nullable: true),
                    DieselQuantityConsumed = table.Column<double>(type: "float", nullable: true),
                    PetrolQuantityConsumed = table.Column<double>(type: "float", nullable: true),
                    QuantityOfDrinkableWaterConsumed = table.Column<double>(type: "float", nullable: true),
                    QuantityOfRegularWaterConsumed = table.Column<double>(type: "float", nullable: true),
                    CostOfRegularWaterConsumed = table.Column<double>(type: "float", nullable: true),
                    CostOfDrinkableWaterConsumed = table.Column<double>(type: "float", nullable: true),
                    NumberOfPaperUsed = table.Column<int>(type: "int", nullable: true),
                    CostOfPaperUsed = table.Column<double>(type: "float", nullable: true),
                    MeansOfDisposal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CostOfDisposal = table.Column<double>(type: "float", nullable: true),
                    QuantityOfDisposal = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FootprintTable", x => x.CarbornFootprint);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FootprintTable");
        }
    }
}
