using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LiveMap.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Feature52_80 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Position",
                table: "Map",
                newName: "Bounds");

            migrationBuilder.AlterColumn<string>(
                name: "CategoryName",
                table: "PointOfInterest",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "EMPTY",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Map",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Map");

            migrationBuilder.RenameColumn(
                name: "Bounds",
                table: "Map",
                newName: "Position");

            migrationBuilder.AlterColumn<string>(
                name: "CategoryName",
                table: "PointOfInterest",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldDefaultValue: "EMPTY");
        }
    }
}
