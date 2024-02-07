using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarbonFootprint1.Migrations
{
    public partial class TableUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DieselGeneratorCO2Emission",
                table: "FootprintTable",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DieselVehicleCO2Emission",
                table: "FootprintTable",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ElectricityCO2Emission",
                table: "FootprintTable",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PetrolCO2Emission",
                table: "FootprintTable",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DieselGeneratorCO2Emission",
                table: "FootprintTable");

            migrationBuilder.DropColumn(
                name: "DieselVehicleCO2Emission",
                table: "FootprintTable");

            migrationBuilder.DropColumn(
                name: "ElectricityCO2Emission",
                table: "FootprintTable");

            migrationBuilder.DropColumn(
                name: "PetrolCO2Emission",
                table: "FootprintTable");
        }
    }
}
