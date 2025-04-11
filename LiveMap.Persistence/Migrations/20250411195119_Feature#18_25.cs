using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LiveMap.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Feature18_25 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PointOfInterest_Category_CategoryName",
                table: "PointOfInterest");

            migrationBuilder.DropForeignKey(
                name: "FK_PointOfInterest_Status_StatusName",
                table: "PointOfInterest");

            migrationBuilder.AlterColumn<string>(
                name: "StatusName",
                table: "PointOfInterest",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CategoryName",
                table: "PointOfInterest",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsWheelchairAccessible",
                table: "PointOfInterest",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_PointOfInterest_Category_CategoryName",
                table: "PointOfInterest",
                column: "CategoryName",
                principalTable: "Category",
                principalColumn: "Category",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PointOfInterest_Status_StatusName",
                table: "PointOfInterest",
                column: "StatusName",
                principalTable: "Status",
                principalColumn: "Status",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PointOfInterest_Category_CategoryName",
                table: "PointOfInterest");

            migrationBuilder.DropForeignKey(
                name: "FK_PointOfInterest_Status_StatusName",
                table: "PointOfInterest");

            migrationBuilder.DropColumn(
                name: "IsWheelchairAccessible",
                table: "PointOfInterest");

            migrationBuilder.AlterColumn<string>(
                name: "StatusName",
                table: "PointOfInterest",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CategoryName",
                table: "PointOfInterest",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_PointOfInterest_Category_CategoryName",
                table: "PointOfInterest",
                column: "CategoryName",
                principalTable: "Category",
                principalColumn: "Category");

            migrationBuilder.AddForeignKey(
                name: "FK_PointOfInterest_Status_StatusName",
                table: "PointOfInterest",
                column: "StatusName",
                principalTable: "Status",
                principalColumn: "Status");
        }
    }
}
