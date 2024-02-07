using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarbonFootprint1.Migrations
{
    public partial class AddToFootprintTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "StaffDieselEmission",
                table: "FootprintTable",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "StaffElectricityEmission",
                table: "FootprintTable",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "StaffPetrolEmission",
                table: "FootprintTable",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalStaffNumber",
                table: "FootprintTable",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StaffDieselEmission",
                table: "FootprintTable");

            migrationBuilder.DropColumn(
                name: "StaffElectricityEmission",
                table: "FootprintTable");

            migrationBuilder.DropColumn(
                name: "StaffPetrolEmission",
                table: "FootprintTable");

            migrationBuilder.DropColumn(
                name: "TotalStaffNumber",
                table: "FootprintTable");
        }
    }
}
