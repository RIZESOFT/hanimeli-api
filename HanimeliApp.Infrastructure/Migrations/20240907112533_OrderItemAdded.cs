using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HanimeliApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OrderItemAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Beverages_Orders_OrderId",
            //    table: "Beverages");

            migrationBuilder.DropForeignKey(
                name: "FK_Menus_Orders_OrderId",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Menus_OrderId",
                table: "Menus");

            //migrationBuilder.DropIndex(
            //    name: "IX_Beverages_OrderId",
            //    table: "Beverages");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Menus");

            //migrationBuilder.DropColumn(
            //    name: "OrderId",
            //    table: "Beverages");

            migrationBuilder.AddColumn<int>(
                name: "FoodId",
                table: "CartItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "OrderItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    MenuId = table.Column<int>(type: "integer", nullable: true),
                    FoodId = table.Column<int>(type: "integer", nullable: true),
                    BeverageId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItem_Beverages_BeverageId",
                        column: x => x.BeverageId,
                        principalTable: "Beverages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderItem_Foods_FoodId",
                        column: x => x.FoodId,
                        principalTable: "Foods",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderItem_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderItem_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_FoodId",
                table: "CartItems",
                column: "FoodId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_BeverageId",
                table: "OrderItem",
                column: "BeverageId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_FoodId",
                table: "OrderItem",
                column: "FoodId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_MenuId",
                table: "OrderItem",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_OrderId",
                table: "OrderItem",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Foods_FoodId",
                table: "CartItems",
                column: "FoodId",
                principalTable: "Foods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Foods_FoodId",
                table: "CartItems");

            migrationBuilder.DropTable(
                name: "OrderItem");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_FoodId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "FoodId",
                table: "CartItems");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Menus",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Beverages",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Menus_OrderId",
                table: "Menus",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Beverages_OrderId",
                table: "Beverages",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Beverages_Orders_OrderId",
                table: "Beverages",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_Orders_OrderId",
                table: "Menus",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
