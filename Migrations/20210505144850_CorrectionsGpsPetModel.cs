using Microsoft.EntityFrameworkCore.Migrations;

namespace PetsApi.Migrations
{
    public partial class CorrectionsGpsPetModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GpsPet_Client_ClientsId",
                table: "GpsPet");

            migrationBuilder.DropForeignKey(
                name: "FK_GpsPet_Pets_PetsId",
                table: "GpsPet");

            migrationBuilder.DropIndex(
                name: "IX_GpsPet_ClientsId",
                table: "GpsPet");

            migrationBuilder.DropIndex(
                name: "IX_GpsPet_PetsId",
                table: "GpsPet");

            migrationBuilder.DropColumn(
                name: "ClientsId",
                table: "GpsPet");

            migrationBuilder.DropColumn(
                name: "PetsId",
                table: "GpsPet");

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "GpsPet",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PetId",
                table: "GpsPet",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GpsPet_ClientId",
                table: "GpsPet",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_GpsPet_PetId",
                table: "GpsPet",
                column: "PetId");

            migrationBuilder.AddForeignKey(
                name: "FK_GpsPet_Client_ClientId",
                table: "GpsPet",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GpsPet_Pets_PetId",
                table: "GpsPet",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GpsPet_Client_ClientId",
                table: "GpsPet");

            migrationBuilder.DropForeignKey(
                name: "FK_GpsPet_Pets_PetId",
                table: "GpsPet");

            migrationBuilder.DropIndex(
                name: "IX_GpsPet_ClientId",
                table: "GpsPet");

            migrationBuilder.DropIndex(
                name: "IX_GpsPet_PetId",
                table: "GpsPet");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "GpsPet");

            migrationBuilder.DropColumn(
                name: "PetId",
                table: "GpsPet");

            migrationBuilder.AddColumn<int>(
                name: "ClientsId",
                table: "GpsPet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PetsId",
                table: "GpsPet",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GpsPet_ClientsId",
                table: "GpsPet",
                column: "ClientsId");

            migrationBuilder.CreateIndex(
                name: "IX_GpsPet_PetsId",
                table: "GpsPet",
                column: "PetsId");

            migrationBuilder.AddForeignKey(
                name: "FK_GpsPet_Client_ClientsId",
                table: "GpsPet",
                column: "ClientsId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GpsPet_Pets_PetsId",
                table: "GpsPet",
                column: "PetsId",
                principalTable: "Pets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
