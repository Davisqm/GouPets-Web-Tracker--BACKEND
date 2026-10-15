using Microsoft.EntityFrameworkCore.Migrations;

namespace PetsApi.Migrations
{
    public partial class RelationSimsAndDevicesTraking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Condition",
                table: "InventorySim",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "DeviceTrakingId",
                table: "InventorySim",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceTrakingImei",
                table: "InventorySim",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Observations",
                table: "InventorySim",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Condition",
                table: "DeviceTraking",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SimId",
                table: "DeviceTraking",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SimNumber",
                table: "DeviceTraking",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventorySim_DeviceTrakingId",
                table: "InventorySim",
                column: "DeviceTrakingId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventorySim_DeviceTraking_DeviceTrakingId",
                table: "InventorySim",
                column: "DeviceTrakingId",
                principalTable: "DeviceTraking",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventorySim_DeviceTraking_DeviceTrakingId",
                table: "InventorySim");

            migrationBuilder.DropIndex(
                name: "IX_InventorySim_DeviceTrakingId",
                table: "InventorySim");

            migrationBuilder.DropColumn(
                name: "Condition",
                table: "InventorySim");

            migrationBuilder.DropColumn(
                name: "DeviceTrakingId",
                table: "InventorySim");

            migrationBuilder.DropColumn(
                name: "DeviceTrakingImei",
                table: "InventorySim");

            migrationBuilder.DropColumn(
                name: "Observations",
                table: "InventorySim");

            migrationBuilder.DropColumn(
                name: "Condition",
                table: "DeviceTraking");

            migrationBuilder.DropColumn(
                name: "SimId",
                table: "DeviceTraking");

            migrationBuilder.DropColumn(
                name: "SimNumber",
                table: "DeviceTraking");
        }
    }
}
