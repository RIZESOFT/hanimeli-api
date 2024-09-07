using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HanimeliApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserCookIdAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CookId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CookId",
                table: "Users",
                column: "CookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Cooks_CookId",
                table: "Users",
                column: "CookId",
                principalTable: "Cooks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Cooks_CookId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CookId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CookId",
                table: "Users");
        }
    }
}
