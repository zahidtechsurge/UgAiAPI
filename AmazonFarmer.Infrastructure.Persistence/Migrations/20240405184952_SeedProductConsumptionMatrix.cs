using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    public partial class SeedProductConsumptionMatrix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductConsumptionMetric",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    CropID = table.Column<int>(type: "int", nullable: false),
                    TerritoryID = table.Column<int>(type: "int", nullable: true),
                    Usage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UOM = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductConsumptionMetric", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProductConsumptionMetric_Crops_CropID",
                        column: x => x.CropID,
                        principalTable: "Crops",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductConsumptionMetric_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5697), new DateTime(2024, 5, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5698) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5701), new DateTime(2024, 5, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5701) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 3,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5703), new DateTime(2024, 5, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5703) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 4,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5705), new DateTime(2024, 5, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5705) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 5,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5707), new DateTime(2024, 5, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5707) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 6,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5709), new DateTime(2024, 5, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5709) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 7,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5710), new DateTime(2024, 5, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5711) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 8,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5712), new DateTime(2024, 5, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5713) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 9,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5714), new DateTime(2024, 5, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5714) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 10,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5716), new DateTime(2024, 5, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5716) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 11,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5717), new DateTime(2024, 5, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5718) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 12,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5724), new DateTime(2024, 5, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5725) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 13,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5726), new DateTime(2024, 5, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5734) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 14,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5750), new DateTime(2024, 5, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5750) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 15,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5751), new DateTime(2024, 5, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5752) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 16,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5753), new DateTime(2024, 5, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5754) });

            migrationBuilder.InsertData(
                table: "ProductConsumptionMetric",
                columns: new[] { "ID", "CropID", "ProductID", "TerritoryID", "UOM", "Usage" },
                values: new object[,]
                {
                    { 1, 1, 1, 1, "Bags", 1.2m },
                    { 2, 1, 2, 1, "Bags", 2m }
                });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5823));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5825));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5827));

            migrationBuilder.UpdateData(
                table: "Season",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 5, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5515), new DateTime(2024, 4, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5500) });

            migrationBuilder.UpdateData(
                table: "Season",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 5, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5522), new DateTime(2024, 4, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5521) });

            migrationBuilder.UpdateData(
                table: "Service",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5870));

            migrationBuilder.UpdateData(
                table: "Service",
                keyColumn: "ID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5872));

            migrationBuilder.UpdateData(
                table: "Service",
                keyColumn: "ID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 5, 23, 49, 50, 327, DateTimeKind.Local).AddTicks(5873));

            migrationBuilder.CreateIndex(
                name: "IX_ProductConsumptionMetric_CropID",
                table: "ProductConsumptionMetric",
                column: "CropID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductConsumptionMetric_ProductID",
                table: "ProductConsumptionMetric",
                column: "ProductID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductConsumptionMetric");

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(1843), new DateTime(2024, 5, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(1845) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(1852), new DateTime(2024, 5, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(1853) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 3,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(1855), new DateTime(2024, 5, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(1856) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 4,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(1858), new DateTime(2024, 5, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(1859) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 5,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(1861), new DateTime(2024, 5, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(1861) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 6,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(1863), new DateTime(2024, 5, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(1863) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 7,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(1865), new DateTime(2024, 5, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(1866) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 8,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(1868), new DateTime(2024, 5, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(1868) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 9,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(1870), new DateTime(2024, 5, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(1870) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 10,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(1872), new DateTime(2024, 5, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(1873) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 11,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(1874), new DateTime(2024, 5, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(1875) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 12,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(1884), new DateTime(2024, 5, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(1886) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 13,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(1888), new DateTime(2024, 5, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(1895) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 14,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(1916), new DateTime(2024, 5, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(1917) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 15,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(1919), new DateTime(2024, 5, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(1920) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 16,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(1922), new DateTime(2024, 5, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(1922) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(2132));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(2135));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(2138));

            migrationBuilder.UpdateData(
                table: "Season",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 5, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(1628), new DateTime(2024, 4, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(1604) });

            migrationBuilder.UpdateData(
                table: "Season",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 5, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(1642), new DateTime(2024, 4, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(1642) });

            migrationBuilder.UpdateData(
                table: "Service",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(2215));

            migrationBuilder.UpdateData(
                table: "Service",
                keyColumn: "ID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(2219));

            migrationBuilder.UpdateData(
                table: "Service",
                keyColumn: "ID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(2220));
        }
    }
}
