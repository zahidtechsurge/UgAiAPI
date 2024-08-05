using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    public partial class updateTblPlanStatusColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FarmChangeRequests_tblFarmers_ApprovedBy",
                table: "FarmChangeRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Farms_tblFarmers_ApprovedBy",
                table: "Farms");

            migrationBuilder.RenameColumn(
                name: "ApprovedDate",
                table: "Farms",
                newName: "UpdatedOn");

            migrationBuilder.RenameColumn(
                name: "ApprovedBy",
                table: "Farms",
                newName: "UpdatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Farms_ApprovedBy",
                table: "Farms",
                newName: "IX_Farms_UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "ApprovedDate",
                table: "FarmChangeRequests",
                newName: "UpdatedOn");

            migrationBuilder.RenameColumn(
                name: "ApprovedBy",
                table: "FarmChangeRequests",
                newName: "UpdatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_FarmChangeRequests_ApprovedBy",
                table: "FarmChangeRequests",
                newName: "IX_FarmChangeRequests_UpdatedBy");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "PlanProduct",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Acreage",
                table: "Farms",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.CreateTable(
                name: "PlanService",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanCropID = table.Column<int>(type: "int", nullable: false),
                    ServiceID = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanService", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PlanService_PlanCrop_PlanCropID",
                        column: x => x.PlanCropID,
                        principalTable: "PlanCrop",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanService_Service_ServiceID",
                        column: x => x.ServiceID,
                        principalTable: "Service",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4111), new DateTime(2024, 5, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4112) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4114), new DateTime(2024, 5, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4115) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 3,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4117), new DateTime(2024, 5, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4117) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 4,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4119), new DateTime(2024, 5, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4119) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 5,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4121), new DateTime(2024, 5, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4121) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 6,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4123), new DateTime(2024, 5, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4123) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 7,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4125), new DateTime(2024, 5, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4125) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 8,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4127), new DateTime(2024, 5, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4128) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 9,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4129), new DateTime(2024, 5, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4130) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 10,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4131), new DateTime(2024, 5, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4132) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 11,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4133), new DateTime(2024, 5, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4134) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 12,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4145), new DateTime(2024, 5, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4146) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 13,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4147), new DateTime(2024, 5, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4152) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 14,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4172), new DateTime(2024, 5, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4172) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 15,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4174), new DateTime(2024, 5, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4174) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 16,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4176), new DateTime(2024, 5, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4176) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4265));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4267));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4269));

            migrationBuilder.UpdateData(
                table: "Season",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 5, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(3961), new DateTime(2024, 4, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(3943) });

            migrationBuilder.UpdateData(
                table: "Season",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 5, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(3968), new DateTime(2024, 4, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(3967) });

            migrationBuilder.UpdateData(
                table: "Service",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4335));

            migrationBuilder.UpdateData(
                table: "Service",
                keyColumn: "ID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4337));

            migrationBuilder.UpdateData(
                table: "Service",
                keyColumn: "ID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 18, 16, 32, 10, 582, DateTimeKind.Local).AddTicks(4339));

            migrationBuilder.CreateIndex(
                name: "IX_PlanService_PlanCropID",
                table: "PlanService",
                column: "PlanCropID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanService_ServiceID",
                table: "PlanService",
                column: "ServiceID");

            migrationBuilder.AddForeignKey(
                name: "FK_FarmChangeRequests_tblFarmers_UpdatedBy",
                table: "FarmChangeRequests",
                column: "UpdatedBy",
                principalTable: "tblFarmers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Farms_tblFarmers_UpdatedBy",
                table: "Farms",
                column: "UpdatedBy",
                principalTable: "tblFarmers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FarmChangeRequests_tblFarmers_UpdatedBy",
                table: "FarmChangeRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Farms_tblFarmers_UpdatedBy",
                table: "Farms");

            migrationBuilder.DropTable(
                name: "PlanService");

            migrationBuilder.RenameColumn(
                name: "UpdatedOn",
                table: "Farms",
                newName: "ApprovedDate");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Farms",
                newName: "ApprovedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Farms_UpdatedBy",
                table: "Farms",
                newName: "IX_Farms_ApprovedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedOn",
                table: "FarmChangeRequests",
                newName: "ApprovedDate");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "FarmChangeRequests",
                newName: "ApprovedBy");

            migrationBuilder.RenameIndex(
                name: "IX_FarmChangeRequests_UpdatedBy",
                table: "FarmChangeRequests",
                newName: "IX_FarmChangeRequests_ApprovedBy");

            migrationBuilder.AlterColumn<string>(
                name: "Date",
                table: "PlanProduct",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<double>(
                name: "Acreage",
                table: "Farms",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

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

            migrationBuilder.AddForeignKey(
                name: "FK_FarmChangeRequests_tblFarmers_ApprovedBy",
                table: "FarmChangeRequests",
                column: "ApprovedBy",
                principalTable: "tblFarmers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Farms_tblFarmers_ApprovedBy",
                table: "Farms",
                column: "ApprovedBy",
                principalTable: "tblFarmers",
                principalColumn: "Id");
        }
    }
}
