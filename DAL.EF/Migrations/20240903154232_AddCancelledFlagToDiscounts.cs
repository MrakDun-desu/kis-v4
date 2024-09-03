using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KisV4.DAL.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddCancelledFlagToDiscounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Containers_ContainerTemplateEntity_TemplateId",
                table: "Containers");

            migrationBuilder.DropForeignKey(
                name: "FK_ContainerTemplateEntity_StoreItems_ContainedItemId",
                table: "ContainerTemplateEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_UserAccounts_ResponsibleUserId",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContainerTemplateEntity",
                table: "ContainerTemplateEntity");

            migrationBuilder.RenameTable(
                name: "ContainerTemplateEntity",
                newName: "ContainerTemplates");

            migrationBuilder.RenameIndex(
                name: "IX_ContainerTemplateEntity_ContainedItemId",
                table: "ContainerTemplates",
                newName: "IX_ContainerTemplates_ContainedItemId");

            migrationBuilder.AlterColumn<int>(
                name: "ResponsibleUserId",
                table: "Transactions",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Discounts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "OpenSince",
                table: "Containers",
                type: "timestamp(0) with time zone",
                precision: 0,
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp(0) with time zone",
                oldPrecision: 0,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContainerTemplates",
                table: "ContainerTemplates",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Containers_ContainerTemplates_TemplateId",
                table: "Containers",
                column: "TemplateId",
                principalTable: "ContainerTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContainerTemplates_StoreItems_ContainedItemId",
                table: "ContainerTemplates",
                column: "ContainedItemId",
                principalTable: "StoreItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_UserAccounts_ResponsibleUserId",
                table: "Transactions",
                column: "ResponsibleUserId",
                principalTable: "UserAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Containers_ContainerTemplates_TemplateId",
                table: "Containers");

            migrationBuilder.DropForeignKey(
                name: "FK_ContainerTemplates_StoreItems_ContainedItemId",
                table: "ContainerTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_UserAccounts_ResponsibleUserId",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContainerTemplates",
                table: "ContainerTemplates");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Discounts");

            migrationBuilder.RenameTable(
                name: "ContainerTemplates",
                newName: "ContainerTemplateEntity");

            migrationBuilder.RenameIndex(
                name: "IX_ContainerTemplates_ContainedItemId",
                table: "ContainerTemplateEntity",
                newName: "IX_ContainerTemplateEntity_ContainedItemId");

            migrationBuilder.AlterColumn<int>(
                name: "ResponsibleUserId",
                table: "Transactions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "OpenSince",
                table: "Containers",
                type: "timestamp(0) with time zone",
                precision: 0,
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp(0) with time zone",
                oldPrecision: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContainerTemplateEntity",
                table: "ContainerTemplateEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Containers_ContainerTemplateEntity_TemplateId",
                table: "Containers",
                column: "TemplateId",
                principalTable: "ContainerTemplateEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContainerTemplateEntity_StoreItems_ContainedItemId",
                table: "ContainerTemplateEntity",
                column: "ContainedItemId",
                principalTable: "StoreItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_UserAccounts_ResponsibleUserId",
                table: "Transactions",
                column: "ResponsibleUserId",
                principalTable: "UserAccounts",
                principalColumn: "Id");
        }
    }
}
