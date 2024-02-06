using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sportle.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class CleanupF1Entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Seasons_SeasonId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Results2024_Events_EventId",
                table: "Results2024");

            migrationBuilder.DropIndex(
                name: "IX_Results2024_EventId",
                table: "Results2024");

            migrationBuilder.AlterColumn<Guid>(
                name: "SeasonId",
                table: "Events",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Seasons_SeasonId",
                table: "Events",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Seasons_SeasonId",
                table: "Events");

            migrationBuilder.AlterColumn<Guid>(
                name: "SeasonId",
                table: "Events",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Results2024_EventId",
                table: "Results2024",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Seasons_SeasonId",
                table: "Events",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Results2024_Events_EventId",
                table: "Results2024",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
