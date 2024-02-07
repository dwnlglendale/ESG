using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarbonFootprint1.Migrations
{
    public partial class AddFootprinttableUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "FuelCO2Emission",
                table: "FootprintTable",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "StaffElectricityConsumption",
                table: "FootprintTable",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "StaffFuelEmission",
                table: "FootprintTable",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "StaffPaperConsumption",
                table: "FootprintTable",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "StaffWasteAccumulation",
                table: "FootprintTable",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "StaffWaterConsumption",
                table: "FootprintTable",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FuelCO2Emission",
                table: "FootprintTable");

            migrationBuilder.DropColumn(
                name: "StaffElectricityConsumption",
                table: "FootprintTable");

            migrationBuilder.DropColumn(
                name: "StaffFuelEmission",
                table: "FootprintTable");

            migrationBuilder.DropColumn(
                name: "StaffPaperConsumption",
                table: "FootprintTable");

            migrationBuilder.DropColumn(
                name: "StaffWasteAccumulation",
                table: "FootprintTable");

            migrationBuilder.DropColumn(
                name: "StaffWaterConsumption",
                table: "FootprintTable");
        }
    }
}
