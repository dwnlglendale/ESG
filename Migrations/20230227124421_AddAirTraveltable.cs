using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarbonFootprint1.Migrations
{
    public partial class AddAirTraveltable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AirTravelTable",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DomesticTravelCount = table.Column<int>(type: "int", nullable: false),
                    InternationalTravelCount = table.Column<int>(type: "int", nullable: false),
                    DomesticTravelCost = table.Column<int>(type: "int", nullable: false),
                    InternationalTravelCost = table.Column<int>(type: "int", nullable: false),
                    DepartureAirport = table.Column<int>(type: "int", nullable: false),
                    ArrivalAirport = table.Column<int>(type: "int", nullable: false),
                    DomDistanceTravelled = table.Column<int>(type: "int", nullable: false),
                    IntDistanceTravelled = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirTravelTable", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AirTravelTable");
        }
    }
}
