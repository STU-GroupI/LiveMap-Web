using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace LiveMap.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Geography : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Maps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Border = table.Column<Polygon>(type: "geometry", nullable: false),
                    Position = table.Column<Point>(type: "geometry", nullable: false),
                    WidthInMeters = table.Column<int>(type: "int", nullable: false),
                    LengthInMeters = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PoIStatusses",
                columns: table => new
                {
                    Status = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoIStatusses", x => x.Status);
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
                        name: "FK_PointOfInterest_Categories_CategoryName",
                        column: x => x.CategoryName,
                        principalTable: "Categories",
                        principalColumn: "Name");
                    table.ForeignKey(
                        name: "FK_PointOfInterest_Maps_MapId",
                        column: x => x.MapId,
                        principalTable: "Maps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PointOfInterest_PoIStatusses_StatusName",
                        column: x => x.StatusName,
                        principalTable: "PoIStatusses",
                        principalColumn: "Status");
                });

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PointOfInterest");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Maps");

            migrationBuilder.DropTable(
                name: "PoIStatusses");
        }
    }
}
