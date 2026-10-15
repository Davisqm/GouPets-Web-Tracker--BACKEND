using Microsoft.EntityFrameworkCore.Migrations;

namespace PetsApi.Migrations
{
    public partial class frm_pet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Antiparasite",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "Chipcode",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "Feed",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "Race",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "Sex",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "Species",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "Vaccine",
                table: "Pets");

            migrationBuilder.AddColumn<string>(
                name: "Birthday",
                table: "Pets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Pets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GpsId",
                table: "Pets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Height",
                table: "Pets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Neutered",
                table: "Pets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RateId",
                table: "Pets",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "StatusPet",
                table: "Pets",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Weight",
                table: "Pets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Birthday",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "GpsId",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "Neutered",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "RateId",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "StatusPet",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Pets");

            migrationBuilder.AddColumn<string>(
                name: "Antiparasite",
                table: "Pets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Chipcode",
                table: "Pets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Date",
                table: "Pets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Feed",
                table: "Pets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Race",
                table: "Pets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sex",
                table: "Pets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Species",
                table: "Pets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Vaccine",
                table: "Pets",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
