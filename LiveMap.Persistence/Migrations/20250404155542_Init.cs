using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace LiveMap.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApprovalStatuses",
                columns: table => new
                {
                    Status = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalStatuses", x => x.Status);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Category = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Category);
                });

            migrationBuilder.CreateTable(
                name: "Map",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Border = table.Column<Polygon>(type: "geometry", nullable: false),
                    Position = table.Column<Point>(type: "geometry", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Map", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    Status = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Status);
                });

            migrationBuilder.CreateTable(
                name: "SuggestedPointOfInterest",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<Point>(type: "geometry", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsWheelchairAccessible = table.Column<bool>(type: "bit", nullable: false),
                    MapId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RFCId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuggestedPointOfInterest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuggestedPointOfInterest_Category_CategoryName",
                        column: x => x.CategoryName,
                        principalTable: "Category",
                        principalColumn: "Category");
                    table.ForeignKey(
                        name: "FK_SuggestedPointOfInterest_Map_MapId",
                        column: x => x.MapId,
                        principalTable: "Map",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PointOfInterest",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<Point>(type: "geometry", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StatusName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MapId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointOfInterest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PointOfInterest_Category_CategoryName",
                        column: x => x.CategoryName,
                        principalTable: "Category",
                        principalColumn: "Category");
                    table.ForeignKey(
                        name: "FK_PointOfInterest_Map_MapId",
                        column: x => x.MapId,
                        principalTable: "Map",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PointOfInterest_Status_StatusName",
                        column: x => x.StatusName,
                        principalTable: "Status",
                        principalColumn: "Status");
                });

            migrationBuilder.CreateTable(
                name: "OpeningHours",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    Start = table.Column<TimeSpan>(type: "time", nullable: false),
                    End = table.Column<TimeSpan>(type: "time", nullable: false),
                    PoiId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpeningHours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpeningHours_PointOfInterest_PoiId",
                        column: x => x.PoiId,
                        principalTable: "PointOfInterest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequestForChange",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PoiId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SuggestedPoiId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApprovalStatus = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmittedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApprovedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestForChange", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestForChange_ApprovalStatuses_ApprovalStatus",
                        column: x => x.ApprovalStatus,
                        principalTable: "ApprovalStatuses",
                        principalColumn: "Status",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestForChange_PointOfInterest_PoiId",
                        column: x => x.PoiId,
                        principalTable: "PointOfInterest",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RequestForChange_SuggestedPointOfInterest_SuggestedPoiId",
                        column: x => x.SuggestedPoiId,
                        principalTable: "SuggestedPointOfInterest",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OpeningHours_PoiId",
                table: "OpeningHours",
                column: "PoiId");

            migrationBuilder.CreateIndex(
                name: "IX_PointOfInterest_CategoryName",
                table: "PointOfInterest",
                column: "CategoryName");

            migrationBuilder.CreateIndex(
                name: "IX_PointOfInterest_MapId",
                table: "PointOfInterest",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_PointOfInterest_StatusName",
                table: "PointOfInterest",
                column: "StatusName");

            migrationBuilder.CreateIndex(
                name: "IX_RequestForChange_ApprovalStatus",
                table: "RequestForChange",
                column: "ApprovalStatus");

            migrationBuilder.CreateIndex(
                name: "IX_RequestForChange_PoiId",
                table: "RequestForChange",
                column: "PoiId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestForChange_SuggestedPoiId",
                table: "RequestForChange",
                column: "SuggestedPoiId",
                unique: true,
                filter: "[SuggestedPoiId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SuggestedPointOfInterest_CategoryName",
                table: "SuggestedPointOfInterest",
                column: "CategoryName");

            migrationBuilder.CreateIndex(
                name: "IX_SuggestedPointOfInterest_MapId",
                table: "SuggestedPointOfInterest",
                column: "MapId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OpeningHours");

            migrationBuilder.DropTable(
                name: "RequestForChange");

            migrationBuilder.DropTable(
                name: "ApprovalStatuses");

            migrationBuilder.DropTable(
                name: "PointOfInterest");

            migrationBuilder.DropTable(
                name: "SuggestedPointOfInterest");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Map");
        }
    }
}
