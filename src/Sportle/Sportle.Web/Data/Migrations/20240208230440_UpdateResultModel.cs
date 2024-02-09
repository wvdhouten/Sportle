using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sportle.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateResultModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PredictedOn",
                table: "Results2024",
                newName: "ModifiedOn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModifiedOn",
                table: "Results2024",
                newName: "PredictedOn");
        }
    }
}
