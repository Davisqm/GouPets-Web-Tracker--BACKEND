using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PetsApi.Migrations
{
    public partial class NewDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceTraking_Pets_PetId",
                table: "DeviceTraking");

            migrationBuilder.DropForeignKey(
                name: "FK_InventorySim_DeviceTraking_DeviceTrakingId",
                table: "InventorySim");

            migrationBuilder.DropIndex(
                name: "IX_InventorySim_DeviceTrakingId",
                table: "InventorySim");

            migrationBuilder.DropIndex(
                name: "IX_DeviceTraking_PetId",
                table: "DeviceTraking");

            migrationBuilder.DropColumn(
                name: "GpsId",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "StatusPet",
                table: "Pets");

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
                name: "PetId",
                table: "DeviceTraking");

            migrationBuilder.DropColumn(
                name: "SimId",
                table: "DeviceTraking");

            migrationBuilder.DropColumn(
                name: "SimNumber",
                table: "DeviceTraking");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Client");

            migrationBuilder.AddColumn<bool>(
                name: "deleted",
                table: "Client",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "GpsPet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    ClientsId = table.Column<int>(nullable: true),
                    PetsId = table.Column<int>(nullable: true),
                    DeviceTrakingId = table.Column<int>(nullable: false),
                    InventorySimId = table.Column<int>(nullable: false),
                    Status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GpsPet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GpsPet_Client_ClientsId",
                        column: x => x.ClientsId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GpsPet_DeviceTraking_DeviceTrakingId",
                        column: x => x.DeviceTrakingId,
                        principalTable: "DeviceTraking",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GpsPet_InventorySim_InventorySimId",
                        column: x => x.InventorySimId,
                        principalTable: "InventorySim",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GpsPet_Pets_PetsId",
                        column: x => x.PetsId,
                        principalTable: "Pets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BoxHistoryPet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    petId = table.Column<int>(nullable: false),
                    GpsPetId = table.Column<int>(nullable: false),
                    InitialDate = table.Column<string>(nullable: true),
                    FinalDate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoxHistoryPet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BoxHistoryPet_GpsPet_GpsPetId",
                        column: x => x.GpsPetId,
                        principalTable: "GpsPet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GpsHistoryLocate",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    latitude = table.Column<string>(nullable: true),
                    longitude = table.Column<string>(nullable: true),
                    GpsPetId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GpsHistoryLocate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GpsHistoryLocate_GpsPet_GpsPetId",
                        column: x => x.GpsPetId,
                        principalTable: "GpsPet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrakingAccess",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    GpsPetId = table.Column<int>(nullable: false),
                    InitialDate = table.Column<DateTime>(nullable: true),
                    FinalDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrakingAccess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrakingAccess_GpsPet_GpsPetId",
                        column: x => x.GpsPetId,
                        principalTable: "GpsPet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoxHistoryPet_GpsPetId",
                table: "BoxHistoryPet",
                column: "GpsPetId");

            migrationBuilder.CreateIndex(
                name: "IX_GpsHistoryLocate_GpsPetId",
                table: "GpsHistoryLocate",
                column: "GpsPetId");

            migrationBuilder.CreateIndex(
                name: "IX_GpsPet_ClientsId",
                table: "GpsPet",
                column: "ClientsId");

            migrationBuilder.CreateIndex(
                name: "IX_GpsPet_DeviceTrakingId",
                table: "GpsPet",
                column: "DeviceTrakingId");

            migrationBuilder.CreateIndex(
                name: "IX_GpsPet_InventorySimId",
                table: "GpsPet",
                column: "InventorySimId");

            migrationBuilder.CreateIndex(
                name: "IX_GpsPet_PetsId",
                table: "GpsPet",
                column: "PetsId");

            migrationBuilder.CreateIndex(
                name: "IX_TrakingAccess_GpsPetId",
                table: "TrakingAccess",
                column: "GpsPetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoxHistoryPet");

            migrationBuilder.DropTable(
                name: "GpsHistoryLocate");

            migrationBuilder.DropTable(
                name: "TrakingAccess");

            migrationBuilder.DropTable(
                name: "GpsPet");

            migrationBuilder.DropColumn(
                name: "deleted",
                table: "Client");

            migrationBuilder.AddColumn<string>(
                name: "GpsId",
                table: "Pets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "StatusPet",
                table: "Pets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "DeviceTrakingId",
                table: "InventorySim",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceTrakingImei",
                table: "InventorySim",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Observations",
                table: "InventorySim",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PetId",
                table: "DeviceTraking",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SimId",
                table: "DeviceTraking",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SimNumber",
                table: "DeviceTraking",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "status",
                table: "Client",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_InventorySim_DeviceTrakingId",
                table: "InventorySim",
                column: "DeviceTrakingId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_InventorySim_DeviceTraking_DeviceTrakingId",
                table: "InventorySim",
                column: "DeviceTrakingId",
                principalTable: "DeviceTraking",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
