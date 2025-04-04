using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace LiveMap.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SuggestedPoI_feature11 : Migration
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
                name: "SuggestedPointOfInterest",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<Point>(type: "geometry", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StatusName = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                    table.ForeignKey(
                        name: "FK_SuggestedPointOfInterest_Status_StatusName",
                        column: x => x.StatusName,
                        principalTable: "Status",
                        principalColumn: "Status");
                });

            migrationBuilder.CreateTable(
                name: "RequestForChange",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PoiId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SuggestedPoiId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApprovalStatus = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubmittedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApprovedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_SuggestedPointOfInterest_StatusName",
                table: "SuggestedPointOfInterest",
                column: "StatusName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestForChange");

            migrationBuilder.DropTable(
                name: "ApprovalStatuses");

            migrationBuilder.DropTable(
                name: "SuggestedPointOfInterest");
        }
    }
}
