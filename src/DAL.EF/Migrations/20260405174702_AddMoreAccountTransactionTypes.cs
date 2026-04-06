using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace KisV4.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreAccountTransactionTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountTransactions_Transactions_SaleTransactionId",
                table: "AccountTransactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountTransactions",
                table: "AccountTransactions");

            migrationBuilder.AlterColumn<int>(
                name: "SaleTransactionId",
                table: "AccountTransactions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "AccountTransactions",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Timestamp",
                table: "AccountTransactions",
                type: "timestamp(0) with time zone",
                precision: 0,
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountTransactions",
                table: "AccountTransactions",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AccountTransactions_AccountId",
                table: "AccountTransactions",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountTransactions_Transactions_SaleTransactionId",
                table: "AccountTransactions",
                column: "SaleTransactionId",
                principalTable: "Transactions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountTransactions_Transactions_SaleTransactionId",
                table: "AccountTransactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountTransactions",
                table: "AccountTransactions");

            migrationBuilder.DropIndex(
                name: "IX_AccountTransactions_AccountId",
                table: "AccountTransactions");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AccountTransactions");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "AccountTransactions");

            migrationBuilder.AlterColumn<int>(
                name: "SaleTransactionId",
                table: "AccountTransactions",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountTransactions",
                table: "AccountTransactions",
                columns: new[] { "AccountId", "SaleTransactionId", "Type" });

            migrationBuilder.AddForeignKey(
                name: "FK_AccountTransactions_Transactions_SaleTransactionId",
                table: "AccountTransactions",
                column: "SaleTransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
