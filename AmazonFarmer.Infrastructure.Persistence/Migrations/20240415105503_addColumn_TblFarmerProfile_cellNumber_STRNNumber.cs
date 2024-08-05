using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    public partial class addColumn_TblFarmerProfile_cellNumber_STRNNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CellNumber",
                table: "FarmerProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "STRNNumber",
                table: "FarmerProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1168), new DateTime(2024, 5, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1169) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1171), new DateTime(2024, 5, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1171) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 3,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1173), new DateTime(2024, 5, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1173) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 4,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1174), new DateTime(2024, 5, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1175) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 5,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1176), new DateTime(2024, 5, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1177) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 6,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1178), new DateTime(2024, 5, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1178) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 7,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1180), new DateTime(2024, 5, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1180) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 8,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1181), new DateTime(2024, 5, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1182) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 9,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1183), new DateTime(2024, 5, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1183) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 10,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1185), new DateTime(2024, 5, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1185) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 11,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1186), new DateTime(2024, 5, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1187) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 12,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1194), new DateTime(2024, 5, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1194) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 13,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1196), new DateTime(2024, 5, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1200) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 14,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1216), new DateTime(2024, 5, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1217) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 15,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1218), new DateTime(2024, 5, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1219) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 16,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1220), new DateTime(2024, 5, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1220) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1288));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1290));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1292));

            migrationBuilder.UpdateData(
                table: "Season",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 5, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1032), new DateTime(2024, 4, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1018) });

            migrationBuilder.UpdateData(
                table: "Season",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 5, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1039), new DateTime(2024, 4, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1039) });

            migrationBuilder.UpdateData(
                table: "Service",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1344));

            migrationBuilder.UpdateData(
                table: "Service",
                keyColumn: "ID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1346));

            migrationBuilder.UpdateData(
                table: "Service",
                keyColumn: "ID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 15, 15, 54, 56, 900, DateTimeKind.Local).AddTicks(1347));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CellNumber",
                table: "FarmerProfile");

            migrationBuilder.DropColumn(
                name: "STRNNumber",
                table: "FarmerProfile");

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
        }
    }
}
