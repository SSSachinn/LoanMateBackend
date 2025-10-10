using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class propertyUpdateInUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Users_User_Id",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "User_Id",
                table: "Customers",
                newName: "VerifiedByAdminId");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_User_Id",
                table: "Customers",
                newName: "IX_Customers_VerifiedByAdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Users_VerifiedByAdminId",
                table: "Customers",
                column: "VerifiedByAdminId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Users_VerifiedByAdminId",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "VerifiedByAdminId",
                table: "Customers",
                newName: "User_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_VerifiedByAdminId",
                table: "Customers",
                newName: "IX_Customers_User_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Users_User_Id",
                table: "Customers",
                column: "User_Id",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
