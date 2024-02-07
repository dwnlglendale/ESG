using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarbonFootprint1.Migrations
{
    public partial class RemoveFuelConsumption : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountPaidForDiesel",
                table: "FootprintTable");

            migrationBuilder.DropColumn(
                name: "AmountPaidForPetrol",
                table: "FootprintTable");

            migrationBuilder.DropColumn(
                name: "DieselCostPerLitre",
                table: "FootprintTable");

            migrationBuilder.DropColumn(
                name: "DieselQuantityConsumed",
                table: "FootprintTable");

            migrationBuilder.DropColumn(
                name: "DieselVehicleCO2Emission",
                table: "FootprintTable");

            migrationBuilder.DropColumn(
                name: "FuelCO2Emission",
                table: "FootprintTable");

            migrationBuilder.DropColumn(
                name: "FuelType",
                table: "FootprintTable");

            migrationBuilder.DropColumn(
                name: "HoursUsedByDifferentBranch",
                table: "FootprintTable");

            migrationBuilder.DropColumn(
                name: "NumberOfVehicles",
                table: "FootprintTable");

            migrationBuilder.DropColumn(
                name: "PetrolCO2Emission",
                table: "FootprintTable");

            migrationBuilder.DropColumn(
                name: "PetrolCostPerLitre",
                table: "FootprintTable");

            migrationBuilder.DropColumn(
                name: "PetrolQuantityConsumed",
                table: "FootprintTable");

            migrationBuilder.DropColumn(
                name: "StaffDieselEmission",
                table: "FootprintTable");

            migrationBuilder.DropColumn(
                name: "StaffFuelEmission",
                table: "FootprintTable");

            migrationBuilder.DropColumn(
                name: "StaffPetrolEmission",
                table: "FootprintTable");

            migrationBuilder.DropColumn(
                name: "VehicleBranchList",
                table: "FootprintTable");

            migrationBuilder.DropColumn(
                name: "VehicleBranchShare",
                table: "FootprintTable");

            migrationBuilder.DropColumn(
                name: "VehicleUsers",
                table: "FootprintTable");

            migrationBuilder.AddColumn<byte[]>(
                name: "FileData",
                table: "FootprintTable",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileData",
                table: "FootprintTable");

            migrationBuilder.AddColumn<double>(
                name: "AmountPaidForDiesel",
                table: "FootprintTable",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AmountPaidForPetrol",
                table: "FootprintTable",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DieselCostPerLitre",
                table: "FootprintTable",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DieselQuantityConsumed",
                table: "FootprintTable",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DieselVehicleCO2Emission",
                table: "FootprintTable",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FuelCO2Emission",
                table: "FootprintTable",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FuelType",
                table: "FootprintTable",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HoursUsedByDifferentBranch",
                table: "FootprintTable",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "NumberOfVehicles",
                table: "FootprintTable",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PetrolCO2Emission",
                table: "FootprintTable",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PetrolCostPerLitre",
                table: "FootprintTable",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PetrolQuantityConsumed",
                table: "FootprintTable",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "StaffDieselEmission",
                table: "FootprintTable",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "StaffFuelEmission",
                table: "FootprintTable",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "StaffPetrolEmission",
                table: "FootprintTable",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleBranchList",
                table: "FootprintTable",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleBranchShare",
                table: "FootprintTable",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VehicleUsers",
                table: "FootprintTable",
                type: "int",
                nullable: true);
        }
    }
}
