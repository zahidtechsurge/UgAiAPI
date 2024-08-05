using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    public partial class updateAccountLockOut : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "OTPExpiredOn",
                table: "tblFarmers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WrongPasswordAttempt",
                table: "tblFarmers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "isAccountLocked",
                table: "tblFarmers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4266), new DateTime(2024, 5, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4267) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4269), new DateTime(2024, 5, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4270) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 3,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4271), new DateTime(2024, 5, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4271) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 4,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4273), new DateTime(2024, 5, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4273) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 5,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4274), new DateTime(2024, 5, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4275) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 6,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4276), new DateTime(2024, 5, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4276) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 7,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4277), new DateTime(2024, 5, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4278) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 8,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4279), new DateTime(2024, 5, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4280) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 9,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4281), new DateTime(2024, 5, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4281) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 10,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4282), new DateTime(2024, 5, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4283) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 11,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4284), new DateTime(2024, 5, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4285) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 12,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4294), new DateTime(2024, 5, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4295) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 13,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4296), new DateTime(2024, 5, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4302) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 14,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4313), new DateTime(2024, 5, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4313) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 15,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4315), new DateTime(2024, 5, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4315) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 16,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4316), new DateTime(2024, 5, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4317) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4399));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4401));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4402));

            migrationBuilder.UpdateData(
                table: "Season",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 5, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4090), new DateTime(2024, 4, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4077) });

            migrationBuilder.UpdateData(
                table: "Season",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 5, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4096), new DateTime(2024, 4, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4096) });

            migrationBuilder.UpdateData(
                table: "Service",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4490));

            migrationBuilder.UpdateData(
                table: "Service",
                keyColumn: "ID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4492));

            migrationBuilder.UpdateData(
                table: "Service",
                keyColumn: "ID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 15, 17, 48, 7, 865, DateTimeKind.Local).AddTicks(4493));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OTPExpiredOn",
                table: "tblFarmers");

            migrationBuilder.DropColumn(
                name: "WrongPasswordAttempt",
                table: "tblFarmers");

            migrationBuilder.DropColumn(
                name: "isAccountLocked",
                table: "tblFarmers");

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
    }
}
