using Microsoft.EntityFrameworkCore.Migrations;

namespace PetsApi.Migrations
{
    public partial class ModelTicketsUltimate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Issue",
                table: "Ticket",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "Ticket",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Issue",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Ticket");
        }
    }
}
