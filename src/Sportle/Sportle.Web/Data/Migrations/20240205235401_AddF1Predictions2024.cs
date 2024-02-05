using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sportle.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddF1Predictions2024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Predictions2024",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PredictedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SprintPP = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SprintP1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SprintFL = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RacePP = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RaceP1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RaceP2 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RaceP3 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RaceP4 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RaceP5 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RaceP6 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RaceP7 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RaceP8 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RaceP9 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RaceP10 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RaceFL = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Points = table.Column<int>(type: "int", nullable: false),
                    EarlyBonus = table.Column<int>(type: "int", nullable: false),
                    SprintBonus = table.Column<int>(type: "int", nullable: false),
                    PositionBonus = table.Column<int>(type: "int", nullable: false),
                    PodiumBonus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Predictions2024", x => new { x.UserId, x.EventId });
                });

            migrationBuilder.CreateTable(
                name: "Results2024",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PredictedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SprintPP = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SprintP1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SprintFL = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RacePP = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RaceP1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RaceP2 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RaceP3 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RaceP4 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RaceP5 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RaceP6 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RaceP7 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RaceP8 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RaceP9 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RaceP10 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RaceFL = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results2024", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Results2024_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Results2024_EventId",
                table: "Results2024",
                column: "EventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Predictions2024");

            migrationBuilder.DropTable(
                name: "Results2024");
        }
    }
}
