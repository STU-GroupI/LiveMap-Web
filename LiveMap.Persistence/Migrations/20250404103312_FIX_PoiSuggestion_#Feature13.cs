using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LiveMap.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FIX_PoiSuggestion_Feature13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SuggestedPointOfInterest_Status_StatusName",
                table: "SuggestedPointOfInterest");

            migrationBuilder.DropIndex(
                name: "IX_SuggestedPointOfInterest_StatusName",
                table: "SuggestedPointOfInterest");

            migrationBuilder.DropColumn(
                name: "StatusName",
                table: "SuggestedPointOfInterest");

            migrationBuilder.AddColumn<bool>(
                name: "IsWheelchairAccessible",
                table: "SuggestedPointOfInterest",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "RequestForChange",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsWheelchairAccessible",
                table: "SuggestedPointOfInterest");

            migrationBuilder.AddColumn<string>(
                name: "StatusName",
                table: "SuggestedPointOfInterest",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "RequestForChange",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SuggestedPointOfInterest_StatusName",
                table: "SuggestedPointOfInterest",
                column: "StatusName");

            migrationBuilder.AddForeignKey(
                name: "FK_SuggestedPointOfInterest_Status_StatusName",
                table: "SuggestedPointOfInterest",
                column: "StatusName",
                principalTable: "Status",
                principalColumn: "Status");
        }
    }
}
