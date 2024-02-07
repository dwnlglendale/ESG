using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarbonFootprint1.Migrations
{
    public partial class UpdateFootprintTable22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DieselGeneratorCO2KGS",
                table: "FootprintTable",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ElectricityCO2KGS",
                table: "FootprintTable",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DieselGeneratorCO2KGS",
                table: "FootprintTable");

            migrationBuilder.DropColumn(
                name: "ElectricityCO2KGS",
                table: "FootprintTable");
        }
    }
}
