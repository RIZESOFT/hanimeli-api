using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HanimeliApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CartItemAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Beverages_Carts_CartId",
                table: "Beverages");

            migrationBuilder.DropForeignKey(
                name: "FK_Menus_Carts_CartId",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Menus_CartId",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Beverages_CartId",
                table: "Beverages");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "Beverages");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Carts",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Carts");

            migrationBuilder.AddColumn<int>(
                name: "CartId",
                table: "Menus",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "Carts",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "CartId",
                table: "Beverages",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Menus_CartId",
                table: "Menus",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_Beverages_CartId",
                table: "Beverages",
                column: "CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Beverages_Carts_CartId",
                table: "Beverages",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_Carts_CartId",
                table: "Menus",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id");
        }
    }
}
