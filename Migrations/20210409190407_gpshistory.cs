using Microsoft.EntityFrameworkCore.Migrations;

namespace PetsApi.Migrations
{
    public partial class gpshistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GpsHistoryLocate_GpsPet_GpsPetId",
                table: "GpsHistoryLocate");

            migrationBuilder.DropIndex(
                name: "IX_GpsHistoryLocate_GpsPetId",
                table: "GpsHistoryLocate");

            migrationBuilder.DropColumn(
                name: "GpsPetId",
                table: "GpsHistoryLocate");

            migrationBuilder.AddColumn<string>(
                name: "addres",
                table: "GpsHistoryLocate",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "gpsPetsId",
                table: "GpsHistoryLocate",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_GpsHistoryLocate_gpsPetsId",
                table: "GpsHistoryLocate",
                column: "gpsPetsId");

            migrationBuilder.AddForeignKey(
                name: "FK_GpsHistoryLocate_GpsPet_gpsPetsId",
                table: "GpsHistoryLocate",
                column: "gpsPetsId",
                principalTable: "GpsPet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GpsHistoryLocate_GpsPet_gpsPetsId",
                table: "GpsHistoryLocate");

            migrationBuilder.DropIndex(
                name: "IX_GpsHistoryLocate_gpsPetsId",
                table: "GpsHistoryLocate");

            migrationBuilder.DropColumn(
                name: "addres",
                table: "GpsHistoryLocate");

            migrationBuilder.DropColumn(
                name: "gpsPetsId",
                table: "GpsHistoryLocate");

            migrationBuilder.AddColumn<int>(
                name: "GpsPetId",
                table: "GpsHistoryLocate",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_GpsHistoryLocate_GpsPetId",
                table: "GpsHistoryLocate",
                column: "GpsPetId");

            migrationBuilder.AddForeignKey(
                name: "FK_GpsHistoryLocate_GpsPet_GpsPetId",
                table: "GpsHistoryLocate",
                column: "GpsPetId",
                principalTable: "GpsPet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
