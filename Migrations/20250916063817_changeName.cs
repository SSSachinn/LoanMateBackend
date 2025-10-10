using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class changeName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Users_VerifiedByAdminId",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "VerifiedByAdminId",
                table: "Customers",
                newName: "CustomerUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_VerifiedByAdminId",
                table: "Customers",
                newName: "IX_Customers_CustomerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Users_CustomerUserId",
                table: "Customers",
                column: "CustomerUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Users_CustomerUserId",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "CustomerUserId",
                table: "Customers",
                newName: "VerifiedByAdminId");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_CustomerUserId",
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
    }
}
