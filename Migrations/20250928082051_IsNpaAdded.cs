using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class IsNpaAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoanApplicationId",
                table: "Npas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsNpa",
                table: "LoanApplications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Npas_LoanApplicationId",
                table: "Npas",
                column: "LoanApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Npas_LoanApplications_LoanApplicationId",
                table: "Npas",
                column: "LoanApplicationId",
                principalTable: "LoanApplications",
                principalColumn: "ApplicationId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Npas_LoanApplications_LoanApplicationId",
                table: "Npas");

            migrationBuilder.DropIndex(
                name: "IX_Npas_LoanApplicationId",
                table: "Npas");

            migrationBuilder.DropColumn(
                name: "LoanApplicationId",
                table: "Npas");

            migrationBuilder.DropColumn(
                name: "IsNpa",
                table: "LoanApplications");
        }
    }
}
