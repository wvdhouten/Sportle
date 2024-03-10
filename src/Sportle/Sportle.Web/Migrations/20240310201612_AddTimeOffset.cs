using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sportle.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddTimeOffset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TimeZoneOffset",
                table: "Sessions",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeZoneOffset",
                table: "Sessions");
        }
    }
}
