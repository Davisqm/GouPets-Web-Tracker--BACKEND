using Microsoft.EntityFrameworkCore.Migrations;

namespace PetsApi.Migrations
{
    public partial class ApiCorrectSuccess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "Specie",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Rate",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Specie",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Rate",
                newName: "name");
        }
    }
}
