using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Triathlon.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddWeightAndHeightToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "HeightCm",
                table: "AspNetUsers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "WeightKg",
                table: "AspNetUsers",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeightCm",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "WeightKg",
                table: "AspNetUsers");
        }
    }
}
