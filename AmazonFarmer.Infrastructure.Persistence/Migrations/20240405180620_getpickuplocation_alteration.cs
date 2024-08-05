using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    public partial class getpickuplocation_alteration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "latitude",
                table: "Farms",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "longitude",
                table: "Farms",
                type: "float",
                nullable: true);

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
                columns: new[] { "CategoryID", "CreatedDate" },
                values: new object[] { 2, new DateTime(2024, 4, 5, 23, 6, 13, 705, DateTimeKind.Local).AddTicks(2138) });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "latitude",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "longitude",
                table: "Farms");

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6154), new DateTime(2024, 5, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6156) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6159), new DateTime(2024, 5, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6161) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 3,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6163), new DateTime(2024, 5, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6164) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 4,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6167), new DateTime(2024, 5, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6168) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 5,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6170), new DateTime(2024, 5, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6171) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 6,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6174), new DateTime(2024, 5, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6175) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 7,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6177), new DateTime(2024, 5, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6178) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 8,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6181), new DateTime(2024, 5, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6182) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 9,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6185), new DateTime(2024, 5, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6186) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 10,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6188), new DateTime(2024, 5, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6189) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 11,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6191), new DateTime(2024, 5, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6192) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 12,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6201), new DateTime(2024, 5, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6203) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 13,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6205), new DateTime(2024, 5, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6213) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 14,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6232), new DateTime(2024, 5, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6233) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 15,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6236), new DateTime(2024, 5, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6237) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 16,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6239), new DateTime(2024, 5, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6240) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6366));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6371));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 3,
                columns: new[] { "CategoryID", "CreatedDate" },
                values: new object[] { 1, new DateTime(2024, 4, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6374) });

            migrationBuilder.UpdateData(
                table: "Season",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 5, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(5932), new DateTime(2024, 4, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(5916) });

            migrationBuilder.UpdateData(
                table: "Season",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 5, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(5944), new DateTime(2024, 4, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(5942) });

            migrationBuilder.UpdateData(
                table: "Service",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6448));

            migrationBuilder.UpdateData(
                table: "Service",
                keyColumn: "ID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6452));

            migrationBuilder.UpdateData(
                table: "Service",
                keyColumn: "ID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 5, 21, 45, 43, 61, DateTimeKind.Local).AddTicks(6454));
        }
    }
}
