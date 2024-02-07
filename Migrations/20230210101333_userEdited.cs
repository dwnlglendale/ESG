using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarbonFootprint1.Migrations
{
    public partial class userEdited : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_BranchesTable_UserBranchBranchID",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UserBranchBranchID",
                table: "AspNetUsers",
                newName: "BranchesBranchID");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_UserBranchBranchID",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_BranchesBranchID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_BranchesTable_BranchesBranchID",
                table: "AspNetUsers",
                column: "BranchesBranchID",
                principalTable: "BranchesTable",
                principalColumn: "BranchID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_BranchesTable_BranchesBranchID",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "BranchesBranchID",
                table: "AspNetUsers",
                newName: "UserBranchBranchID");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_BranchesBranchID",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_UserBranchBranchID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_BranchesTable_UserBranchBranchID",
                table: "AspNetUsers",
                column: "UserBranchBranchID",
                principalTable: "BranchesTable",
                principalColumn: "BranchID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
