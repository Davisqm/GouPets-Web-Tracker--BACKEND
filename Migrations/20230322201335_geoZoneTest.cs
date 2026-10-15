using Microsoft.EntityFrameworkCore.Migrations;

namespace PetsApi.Migrations
{
    public partial class geoZoneTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeoZona_GpsPet_GpsPetId",
                table: "GeoZona");

            migrationBuilder.AlterColumn<int>(
                name: "GpsPetId",
                table: "GeoZona",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_GeoZona_GpsPet_GpsPetId",
                table: "GeoZona",
                column: "GpsPetId",
                principalTable: "GpsPet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeoZona_GpsPet_GpsPetId",
                table: "GeoZona");

            migrationBuilder.AlterColumn<int>(
                name: "GpsPetId",
                table: "GeoZona",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GeoZona_GpsPet_GpsPetId",
                table: "GeoZona",
                column: "GpsPetId",
                principalTable: "GpsPet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
