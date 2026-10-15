using Microsoft.EntityFrameworkCore.Migrations;

namespace PetsApi.Migrations
{
    public partial class raceAndSpecie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Specie");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Rate");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "Specie",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpecieId",
                table: "Rate",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "Rate",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RateId",
                table: "Pets",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpecieId",
                table: "Pets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Rate_SpecieId",
                table: "Rate",
                column: "SpecieId");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_SpecieId",
                table: "Pets",
                column: "SpecieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_Specie_SpecieId",
                table: "Pets",
                column: "SpecieId",
                principalTable: "Specie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rate_Specie_SpecieId",
                table: "Rate",
                column: "SpecieId",
                principalTable: "Specie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pets_Specie_SpecieId",
                table: "Pets");

            migrationBuilder.DropForeignKey(
                name: "FK_Rate_Specie_SpecieId",
                table: "Rate");

            migrationBuilder.DropIndex(
                name: "IX_Rate_SpecieId",
                table: "Rate");

            migrationBuilder.DropIndex(
                name: "IX_Pets_SpecieId",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "name",
                table: "Specie");

            migrationBuilder.DropColumn(
                name: "SpecieId",
                table: "Rate");

            migrationBuilder.DropColumn(
                name: "name",
                table: "Rate");

            migrationBuilder.DropColumn(
                name: "SpecieId",
                table: "Pets");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Specie",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Rate",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RateId",
                table: "Pets",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
