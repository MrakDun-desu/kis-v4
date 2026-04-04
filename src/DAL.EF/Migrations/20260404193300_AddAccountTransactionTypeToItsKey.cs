using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KisV4.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountTransactionTypeToItsKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountTransactions",
                table: "AccountTransactions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountTransactions",
                table: "AccountTransactions",
                columns: new[] { "AccountId", "SaleTransactionId", "Type" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountTransactions",
                table: "AccountTransactions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountTransactions",
                table: "AccountTransactions",
                columns: new[] { "AccountId", "SaleTransactionId" });
        }
    }
}
