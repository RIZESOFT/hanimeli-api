using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HanimeliApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class B2BSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvailableWeekDays",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DailyOrderCount",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderDays",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderHours",
                table: "Users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableWeekDays",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DailyOrderCount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OrderDays",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OrderHours",
                table: "Users");
        }
    }
}
