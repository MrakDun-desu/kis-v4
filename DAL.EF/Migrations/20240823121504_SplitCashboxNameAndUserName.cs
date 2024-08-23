using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KisV4.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class SplitCashboxNameAndUserName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Accounts");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "UserAccounts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CashBoxes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_UserName",
                table: "UserAccounts",
                column: "UserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserAccounts_UserName",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "CashBoxes");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Accounts",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
