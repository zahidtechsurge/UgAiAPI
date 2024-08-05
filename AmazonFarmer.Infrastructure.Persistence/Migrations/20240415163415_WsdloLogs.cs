using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    public partial class WsdloLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WSDLLogs",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HttpMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestBody = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseTimestamp = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WSDLLogs", x => x.RequestId);
                });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 21, 34, 14, 51, DateTimeKind.Local).AddTicks(2462), new DateTime(2024, 5, 15, 21, 34, 14, 51, DateTimeKind.Local).AddTicks(2463) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 21, 34, 14, 51, DateTimeKind.Local).AddTicks(2464), new DateTime(2024, 5, 15, 21, 34, 14, 51, DateTimeKind.Local).AddTicks(2464) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 3,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 21, 34, 14, 51, DateTimeKind.Local).AddTicks(2465), new DateTime(2024, 5, 15, 21, 34, 14, 51, DateTimeKind.Local).AddTicks(2465) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 4,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 21, 34, 14, 51, DateTimeKind.Local).AddTicks(2466), new DateTime(2024, 5, 15, 21, 34, 14, 51, DateTimeKind.Local).AddTicks(2467) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 5,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 21, 34, 14, 51, DateTimeKind.Local).AddTicks(2467), new DateTime(2024, 5, 15, 21, 34, 14, 51, DateTimeKind.Local).AddTicks(2468) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 6,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 21, 34, 14, 51, DateTimeKind.Local).AddTicks(2469), new DateTime(2024, 5, 15, 21, 34, 14, 51, DateTimeKind.Local).AddTicks(2469) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 7,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 21, 34, 14, 51, DateTimeKind.Local).AddTicks(2470), new DateTime(2024, 5, 15, 21, 34, 14, 51, DateTimeKind.Local).AddTicks(2470) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 8,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 21, 34, 14, 51, DateTimeKind.Local).AddTicks(2471), new DateTime(2024, 5, 15, 21, 34, 14, 51, DateTimeKind.Local).AddTicks(2471) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 9,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 21, 34, 14, 51, DateTimeKind.Local).AddTicks(2472), new DateTime(2024, 5, 15, 21, 34, 14, 51, DateTimeKind.Local).AddTicks(2472) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 10,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 21, 34, 14, 51, DateTimeKind.Local).AddTicks(2473), new DateTime(2024, 5, 15, 21, 34, 14, 51, DateTimeKind.Local).AddTicks(2474) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 11,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 21, 34, 14, 51, DateTimeKind.Local).AddTicks(2475), new DateTime(2024, 5, 15, 21, 34, 14, 51, DateTimeKind.Local).AddTicks(2475) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 12,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 21, 34, 14, 51, DateTimeKind.Local).AddTicks(2476), new DateTime(2024, 5, 15, 21, 34, 14, 51, DateTimeKind.Local).AddTicks(2477) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 13,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 21, 34, 14, 51, DateTimeKind.Local).AddTicks(2477), new DateTime(2024, 5, 15, 21, 34, 14, 51, DateTimeKind.Local).AddTicks(2478) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 14,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 21, 34, 14, 51, DateTimeKind.Local).AddTicks(2478), new DateTime(2024, 5, 15, 21, 34, 14, 51, DateTimeKind.Local).AddTicks(2479) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 15,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 21, 34, 14, 51, DateTimeKind.Local).AddTicks(2480), new DateTime(2024, 5, 15, 21, 34, 14, 51, DateTimeKind.Local).AddTicks(2480) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 16,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 21, 34, 14, 51, DateTimeKind.Local).AddTicks(2481), new DateTime(2024, 5, 15, 21, 34, 14, 51, DateTimeKind.Local).AddTicks(2481) });

            migrationBuilder.UpdateData(
                table: "Season",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 5, 15, 21, 34, 14, 51, DateTimeKind.Local).AddTicks(2337), new DateTime(2024, 4, 15, 21, 34, 14, 51, DateTimeKind.Local).AddTicks(2325) });

            migrationBuilder.UpdateData(
                table: "Season",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 5, 15, 21, 34, 14, 51, DateTimeKind.Local).AddTicks(2341), new DateTime(2024, 4, 15, 21, 34, 14, 51, DateTimeKind.Local).AddTicks(2341) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WSDLLogs");

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 3, 19, 46, 45, 856, DateTimeKind.Local).AddTicks(9152), new DateTime(2024, 5, 3, 19, 46, 45, 856, DateTimeKind.Local).AddTicks(9152) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 3, 19, 46, 45, 856, DateTimeKind.Local).AddTicks(9153), new DateTime(2024, 5, 3, 19, 46, 45, 856, DateTimeKind.Local).AddTicks(9154) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 3,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 3, 19, 46, 45, 856, DateTimeKind.Local).AddTicks(9155), new DateTime(2024, 5, 3, 19, 46, 45, 856, DateTimeKind.Local).AddTicks(9155) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 4,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 3, 19, 46, 45, 856, DateTimeKind.Local).AddTicks(9156), new DateTime(2024, 5, 3, 19, 46, 45, 856, DateTimeKind.Local).AddTicks(9156) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 5,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 3, 19, 46, 45, 856, DateTimeKind.Local).AddTicks(9157), new DateTime(2024, 5, 3, 19, 46, 45, 856, DateTimeKind.Local).AddTicks(9157) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 6,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 3, 19, 46, 45, 856, DateTimeKind.Local).AddTicks(9158), new DateTime(2024, 5, 3, 19, 46, 45, 856, DateTimeKind.Local).AddTicks(9158) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 7,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 3, 19, 46, 45, 856, DateTimeKind.Local).AddTicks(9159), new DateTime(2024, 5, 3, 19, 46, 45, 856, DateTimeKind.Local).AddTicks(9160) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 8,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 3, 19, 46, 45, 856, DateTimeKind.Local).AddTicks(9160), new DateTime(2024, 5, 3, 19, 46, 45, 856, DateTimeKind.Local).AddTicks(9161) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 9,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 3, 19, 46, 45, 856, DateTimeKind.Local).AddTicks(9161), new DateTime(2024, 5, 3, 19, 46, 45, 856, DateTimeKind.Local).AddTicks(9162) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 10,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 3, 19, 46, 45, 856, DateTimeKind.Local).AddTicks(9163), new DateTime(2024, 5, 3, 19, 46, 45, 856, DateTimeKind.Local).AddTicks(9163) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 11,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 3, 19, 46, 45, 856, DateTimeKind.Local).AddTicks(9164), new DateTime(2024, 5, 3, 19, 46, 45, 856, DateTimeKind.Local).AddTicks(9164) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 12,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 3, 19, 46, 45, 856, DateTimeKind.Local).AddTicks(9165), new DateTime(2024, 5, 3, 19, 46, 45, 856, DateTimeKind.Local).AddTicks(9165) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 13,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 3, 19, 46, 45, 856, DateTimeKind.Local).AddTicks(9166), new DateTime(2024, 5, 3, 19, 46, 45, 856, DateTimeKind.Local).AddTicks(9166) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 14,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 3, 19, 46, 45, 856, DateTimeKind.Local).AddTicks(9167), new DateTime(2024, 5, 3, 19, 46, 45, 856, DateTimeKind.Local).AddTicks(9167) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 15,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 3, 19, 46, 45, 856, DateTimeKind.Local).AddTicks(9168), new DateTime(2024, 5, 3, 19, 46, 45, 856, DateTimeKind.Local).AddTicks(9169) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 16,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 3, 19, 46, 45, 856, DateTimeKind.Local).AddTicks(9169), new DateTime(2024, 5, 3, 19, 46, 45, 856, DateTimeKind.Local).AddTicks(9170) });

            migrationBuilder.UpdateData(
                table: "Season",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 5, 3, 19, 46, 45, 856, DateTimeKind.Local).AddTicks(9006), new DateTime(2024, 4, 3, 19, 46, 45, 856, DateTimeKind.Local).AddTicks(8992) });

            migrationBuilder.UpdateData(
                table: "Season",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 5, 3, 19, 46, 45, 856, DateTimeKind.Local).AddTicks(9011), new DateTime(2024, 4, 3, 19, 46, 45, 856, DateTimeKind.Local).AddTicks(9010) });
        }
    }
}
