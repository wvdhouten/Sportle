using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sportle.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedScoresToDouble : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "PositionBonus",
                table: "Predictions2024",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PositionBonus",
                table: "Predictions2024",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
