using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class updatedPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanApplications_LoanSheme_SchemeId",
                table: "LoanApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_LoanApplications_loanOfficers_AssignedOfficerId",
                table: "LoanApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_loanOfficers_Users_UserId",
                table: "loanOfficers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_loanOfficers",
                table: "loanOfficers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoanSheme",
                table: "LoanSheme");

            migrationBuilder.RenameTable(
                name: "loanOfficers",
                newName: "LoanOfficers");

            migrationBuilder.RenameTable(
                name: "LoanSheme",
                newName: "LoanSchemes");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Users",
                newName: "Password");

            migrationBuilder.RenameIndex(
                name: "IX_loanOfficers_UserId",
                table: "LoanOfficers",
                newName: "IX_LoanOfficers_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoanOfficers",
                table: "LoanOfficers",
                column: "OfficerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoanSchemes",
                table: "LoanSchemes",
                column: "SchemeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanApplications_LoanOfficers_AssignedOfficerId",
                table: "LoanApplications",
                column: "AssignedOfficerId",
                principalTable: "LoanOfficers",
                principalColumn: "OfficerId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanApplications_LoanSchemes_SchemeId",
                table: "LoanApplications",
                column: "SchemeId",
                principalTable: "LoanSchemes",
                principalColumn: "SchemeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LoanOfficers_Users_UserId",
                table: "LoanOfficers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanApplications_LoanOfficers_AssignedOfficerId",
                table: "LoanApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_LoanApplications_LoanSchemes_SchemeId",
                table: "LoanApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_LoanOfficers_Users_UserId",
                table: "LoanOfficers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoanOfficers",
                table: "LoanOfficers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoanSchemes",
                table: "LoanSchemes");

            migrationBuilder.RenameTable(
                name: "LoanOfficers",
                newName: "loanOfficers");

            migrationBuilder.RenameTable(
                name: "LoanSchemes",
                newName: "LoanSheme");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "PasswordHash");

            migrationBuilder.RenameIndex(
                name: "IX_LoanOfficers_UserId",
                table: "loanOfficers",
                newName: "IX_loanOfficers_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_loanOfficers",
                table: "loanOfficers",
                column: "OfficerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoanSheme",
                table: "LoanSheme",
                column: "SchemeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanApplications_LoanSheme_SchemeId",
                table: "LoanApplications",
                column: "SchemeId",
                principalTable: "LoanSheme",
                principalColumn: "SchemeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LoanApplications_loanOfficers_AssignedOfficerId",
                table: "LoanApplications",
                column: "AssignedOfficerId",
                principalTable: "loanOfficers",
                principalColumn: "OfficerId");

            migrationBuilder.AddForeignKey(
                name: "FK_loanOfficers_Users_UserId",
                table: "loanOfficers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
