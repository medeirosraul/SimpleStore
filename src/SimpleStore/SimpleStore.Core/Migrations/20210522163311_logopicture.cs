using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SimpleStore.Core.Migrations
{
    public partial class logopicture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreSettings");

            migrationBuilder.AddColumn<string>(
                name: "LogoPictureId",
                table: "Stores",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stores_LogoPictureId",
                table: "Stores",
                column: "LogoPictureId",
                unique: true,
                filter: "[LogoPictureId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Pictures_LogoPictureId",
                table: "Stores",
                column: "LogoPictureId",
                principalTable: "Pictures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Pictures_LogoPictureId",
                table: "Stores");

            migrationBuilder.DropIndex(
                name: "IX_Stores_LogoPictureId",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "LogoPictureId",
                table: "Stores");

            migrationBuilder.CreateTable(
                name: "StoreSettings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    LogoPictureId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StoreId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreSettings_Pictures_LogoPictureId",
                        column: x => x.LogoPictureId,
                        principalTable: "Pictures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoreSettings_LogoPictureId",
                table: "StoreSettings",
                column: "LogoPictureId",
                unique: true,
                filter: "[LogoPictureId] IS NOT NULL");
        }
    }
}
