using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    public partial class RequestResponseLogging : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestLogs",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HttpMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestLogs", x => x.RequestId);
                });

            migrationBuilder.CreateTable(
                name: "ResponseLogs",
                columns: table => new
                {
                    ResponseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    StatusCode = table.Column<int>(type: "int", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResponseLogs", x => x.ResponseId);
                    table.ForeignKey(
                        name: "FK_ResponseLogs_RequestLogs_RequestId",
                        column: x => x.RequestId,
                        principalTable: "RequestLogs",
                        principalColumn: "RequestId",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_ResponseLogs_RequestId",
                table: "ResponseLogs",
                column: "RequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResponseLogs");

            migrationBuilder.DropTable(
                name: "RequestLogs");

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 3, 28, 11, 50, 29, 127, DateTimeKind.Local).AddTicks(7764), new DateTime(2024, 4, 28, 11, 50, 29, 127, DateTimeKind.Local).AddTicks(7765) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 3, 28, 11, 50, 29, 127, DateTimeKind.Local).AddTicks(7768), new DateTime(2024, 4, 28, 11, 50, 29, 127, DateTimeKind.Local).AddTicks(7768) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 3,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 3, 28, 11, 50, 29, 127, DateTimeKind.Local).AddTicks(7770), new DateTime(2024, 4, 28, 11, 50, 29, 127, DateTimeKind.Local).AddTicks(7770) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 4,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 3, 28, 11, 50, 29, 127, DateTimeKind.Local).AddTicks(7771), new DateTime(2024, 4, 28, 11, 50, 29, 127, DateTimeKind.Local).AddTicks(7772) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 5,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 3, 28, 11, 50, 29, 127, DateTimeKind.Local).AddTicks(7773), new DateTime(2024, 4, 28, 11, 50, 29, 127, DateTimeKind.Local).AddTicks(7774) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 6,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 3, 28, 11, 50, 29, 127, DateTimeKind.Local).AddTicks(7775), new DateTime(2024, 4, 28, 11, 50, 29, 127, DateTimeKind.Local).AddTicks(7776) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 7,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 3, 28, 11, 50, 29, 127, DateTimeKind.Local).AddTicks(7777), new DateTime(2024, 4, 28, 11, 50, 29, 127, DateTimeKind.Local).AddTicks(7777) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 8,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 3, 28, 11, 50, 29, 127, DateTimeKind.Local).AddTicks(7779), new DateTime(2024, 4, 28, 11, 50, 29, 127, DateTimeKind.Local).AddTicks(7779) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 9,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 3, 28, 11, 50, 29, 127, DateTimeKind.Local).AddTicks(7780), new DateTime(2024, 4, 28, 11, 50, 29, 127, DateTimeKind.Local).AddTicks(7781) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 10,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 3, 28, 11, 50, 29, 127, DateTimeKind.Local).AddTicks(7782), new DateTime(2024, 4, 28, 11, 50, 29, 127, DateTimeKind.Local).AddTicks(7783) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 11,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 3, 28, 11, 50, 29, 127, DateTimeKind.Local).AddTicks(7784), new DateTime(2024, 4, 28, 11, 50, 29, 127, DateTimeKind.Local).AddTicks(7785) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 12,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 3, 28, 11, 50, 29, 127, DateTimeKind.Local).AddTicks(7786), new DateTime(2024, 4, 28, 11, 50, 29, 127, DateTimeKind.Local).AddTicks(7787) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 13,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 3, 28, 11, 50, 29, 127, DateTimeKind.Local).AddTicks(7788), new DateTime(2024, 4, 28, 11, 50, 29, 127, DateTimeKind.Local).AddTicks(7789) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 14,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 3, 28, 11, 50, 29, 127, DateTimeKind.Local).AddTicks(7790), new DateTime(2024, 4, 28, 11, 50, 29, 127, DateTimeKind.Local).AddTicks(7790) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 15,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 3, 28, 11, 50, 29, 127, DateTimeKind.Local).AddTicks(7792), new DateTime(2024, 4, 28, 11, 50, 29, 127, DateTimeKind.Local).AddTicks(7792) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 16,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 3, 28, 11, 50, 29, 127, DateTimeKind.Local).AddTicks(7794), new DateTime(2024, 4, 28, 11, 50, 29, 127, DateTimeKind.Local).AddTicks(7794) });

            migrationBuilder.UpdateData(
                table: "Season",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 4, 28, 11, 50, 29, 127, DateTimeKind.Local).AddTicks(7590), new DateTime(2024, 3, 28, 11, 50, 29, 127, DateTimeKind.Local).AddTicks(7578) });

            migrationBuilder.UpdateData(
                table: "Season",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 4, 28, 11, 50, 29, 127, DateTimeKind.Local).AddTicks(7597), new DateTime(2024, 3, 28, 11, 50, 29, 127, DateTimeKind.Local).AddTicks(7596) });
        }
    }
}
