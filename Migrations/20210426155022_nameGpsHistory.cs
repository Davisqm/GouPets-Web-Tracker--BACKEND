using Microsoft.EntityFrameworkCore.Migrations;

namespace PetsApi.Migrations
{
    public partial class nameGpsHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "longitude",
                table: "GpsHistoryLocate",
                newName: "Longitude");

            migrationBuilder.RenameColumn(
                name: "latitude",
                table: "GpsHistoryLocate",
                newName: "Latitude");

            migrationBuilder.RenameColumn(
                name: "addres",
                table: "GpsHistoryLocate",
                newName: "Addres");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "GpsHistoryLocate",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "GpsHistoryLocate");

            migrationBuilder.RenameColumn(
                name: "Longitude",
                table: "GpsHistoryLocate",
                newName: "longitude");

            migrationBuilder.RenameColumn(
                name: "Latitude",
                table: "GpsHistoryLocate",
                newName: "latitude");

            migrationBuilder.RenameColumn(
                name: "Addres",
                table: "GpsHistoryLocate",
                newName: "addres");
        }
    }
}
