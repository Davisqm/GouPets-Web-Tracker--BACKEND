using Microsoft.EntityFrameworkCore.Migrations;

namespace PetsApi.Migrations
{
    public partial class relacionClientsPets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "user_id",
                table: "Pets");

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Pets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Pets_ClientId",
                table: "Pets",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_Client_ClientId",
                table: "Pets",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pets_Client_ClientId",
                table: "Pets");

            migrationBuilder.DropIndex(
                name: "IX_Pets_ClientId",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Pets");

            migrationBuilder.AddColumn<string>(
                name: "user_id",
                table: "Pets",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
