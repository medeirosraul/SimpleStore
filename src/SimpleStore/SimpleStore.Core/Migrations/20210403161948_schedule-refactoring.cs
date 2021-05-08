using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SimpleStore.Core.Migrations
{
    public partial class schedulerefactoring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleActivities_SchedulePeriod_PeriodId",
                table: "ScheduleActivities");

            migrationBuilder.DropTable(
                name: "SchedulePeriod");

            migrationBuilder.DropTable(
                name: "ScheduleDays");

            migrationBuilder.RenameColumn(
                name: "Effort",
                table: "ScheduleActivities",
                newName: "Duration");

            migrationBuilder.CreateTable(
                name: "ScheduleDates",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScheduleId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StoreId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleDates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduleDates_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SchedulePeriods",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Init = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Granularity = table.Column<int>(type: "int", nullable: false, defaultValue: 60),
                    DateId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StoreId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchedulePeriods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchedulePeriods_ScheduleDates_DateId",
                        column: x => x.DateId,
                        principalTable: "ScheduleDates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleDates_ScheduleId",
                table: "ScheduleDates",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_SchedulePeriods_DateId",
                table: "SchedulePeriods",
                column: "DateId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleActivities_SchedulePeriods_PeriodId",
                table: "ScheduleActivities",
                column: "PeriodId",
                principalTable: "SchedulePeriods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleActivities_SchedulePeriods_PeriodId",
                table: "ScheduleActivities");

            migrationBuilder.DropTable(
                name: "SchedulePeriods");

            migrationBuilder.DropTable(
                name: "ScheduleDates");

            migrationBuilder.RenameColumn(
                name: "Duration",
                table: "ScheduleActivities",
                newName: "Effort");

            migrationBuilder.CreateTable(
                name: "ScheduleDays",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScheduleId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StoreId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduleDays_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SchedulePeriod",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DayId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    End = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Granularity = table.Column<int>(type: "int", nullable: false, defaultValue: 60),
                    Init = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StoreId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchedulePeriod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchedulePeriod_ScheduleDays_DayId",
                        column: x => x.DayId,
                        principalTable: "ScheduleDays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleDays_ScheduleId",
                table: "ScheduleDays",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_SchedulePeriod_DayId",
                table: "SchedulePeriod",
                column: "DayId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleActivities_SchedulePeriod_PeriodId",
                table: "ScheduleActivities",
                column: "PeriodId",
                principalTable: "SchedulePeriod",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
