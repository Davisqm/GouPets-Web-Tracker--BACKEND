using Microsoft.EntityFrameworkCore.Migrations;

namespace PetsApi.Migrations
{
    public partial class geoZoneTest1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeoZona_GpsPet_GpsPetId",
                table: "GeoZona");

            migrationBuilder.DropIndex(
                name: "IX_GeoZona_GpsPetId",
                table: "GeoZona");

            migrationBuilder.DropColumn(
                name: "GpsPetId",
                table: "GeoZona");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GpsPetId",
                table: "GeoZona",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GeoZona_GpsPetId",
                table: "GeoZona",
                column: "GpsPetId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeoZona_GpsPet_GpsPetId",
                table: "GeoZona",
                column: "GpsPetId",
                principalTable: "GpsPet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
