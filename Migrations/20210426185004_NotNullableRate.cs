using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PetsApi.Migrations
{
    public partial class NotNullableRate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rate_Specie_SpecieId",
                table: "Rate");

            migrationBuilder.AlterColumn<int>(
                name: "SpecieId",
                table: "Rate",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "AdvertisingBanner",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Image = table.Column<string>(nullable: true),
                    Link = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    CutoffDate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertisingBanner", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Rate_Specie_SpecieId",
                table: "Rate",
                column: "SpecieId",
                principalTable: "Specie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rate_Specie_SpecieId",
                table: "Rate");

            migrationBuilder.DropTable(
                name: "AdvertisingBanner");

            migrationBuilder.AlterColumn<int>(
                name: "SpecieId",
                table: "Rate",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Rate_Specie_SpecieId",
                table: "Rate",
                column: "SpecieId",
                principalTable: "Specie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
