using Microsoft.EntityFrameworkCore.Migrations;

namespace PetsApi.Migrations
{
    public partial class CorrectionsClientModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "phone",
                table: "Client",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "lastName",
                table: "Client",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "firstName",
                table: "Client",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "document",
                table: "Client",
                newName: "Document");

            migrationBuilder.RenameColumn(
                name: "deleted",
                table: "Client",
                newName: "Deleted");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "Client",
                newName: "Address");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Client",
                newName: "phone");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Client",
                newName: "lastName");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Client",
                newName: "firstName");

            migrationBuilder.RenameColumn(
                name: "Document",
                table: "Client",
                newName: "document");

            migrationBuilder.RenameColumn(
                name: "Deleted",
                table: "Client",
                newName: "deleted");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Client",
                newName: "address");
        }
    }
}
