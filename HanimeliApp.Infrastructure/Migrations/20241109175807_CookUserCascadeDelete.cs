using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HanimeliApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CookUserCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Cooks_CookId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CookId",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CookId",
                table: "Users",
                column: "CookId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Cooks_CookId",
                table: "Users",
                column: "CookId",
                principalTable: "Cooks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
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
    }
}
