using Microsoft.EntityFrameworkCore.Migrations;

namespace PetsApi.Migrations
{
    public partial class InventoryGpsRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Imei",
                table: "DeviceTraking",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PetId",
                table: "DeviceTraking",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeviceTraking_PetId",
                table: "DeviceTraking",
                column: "PetId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceTraking_Pets_PetId",
                table: "DeviceTraking",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceTraking_Pets_PetId",
                table: "DeviceTraking");

            migrationBuilder.DropIndex(
                name: "IX_DeviceTraking_PetId",
                table: "DeviceTraking");

            migrationBuilder.DropColumn(
                name: "Imei",
                table: "DeviceTraking");

            migrationBuilder.DropColumn(
                name: "PetId",
                table: "DeviceTraking");
        }
    }
}
