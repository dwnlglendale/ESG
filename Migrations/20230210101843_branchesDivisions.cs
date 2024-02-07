using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarbonFootprint1.Migrations
{
    public partial class branchesDivisions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_BranchesTable_BranchesBranchID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BranchesBranchID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BranchID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BranchesBranchID",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "BranchCode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BranchName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BranchSize",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BranchCode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BranchName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BranchSize",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "BranchID",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BranchesBranchID",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BranchesBranchID",
                table: "AspNetUsers",
                column: "BranchesBranchID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_BranchesTable_BranchesBranchID",
                table: "AspNetUsers",
                column: "BranchesBranchID",
                principalTable: "BranchesTable",
                principalColumn: "BranchID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
