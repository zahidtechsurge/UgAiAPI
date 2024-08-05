using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class orderservices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducts_PlanProduct_PlanProductID",
                table: "OrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_OrderProducts_PlanProductID",
                table: "OrderProducts");

            migrationBuilder.AddColumn<DateTime>(
                name: "LandPreparationDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastHarvestDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "SewingDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "PlanProductID",
                table: "OrderProducts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "OrderServices",
                columns: table => new
                {
                    OrderServiceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<long>(type: "bigint", nullable: false),
                    ServiceID = table.Column<int>(type: "int", nullable: false),
                    PlanServiceID = table.Column<int>(type: "int", nullable: true),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitTotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QTY = table.Column<int>(type: "int", nullable: false),
                    ClosingQTY = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderServices", x => x.OrderServiceID);
                    table.ForeignKey(
                        name: "FK_OrderServices_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderServices_PlanService_PlanServiceID",
                        column: x => x.PlanServiceID,
                        principalTable: "PlanService",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_OrderServices_Service_ServiceID",
                        column: x => x.ServiceID,
                        principalTable: "Service",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_PlanProductID",
                table: "OrderProducts",
                column: "PlanProductID",
                unique: true,
                filter: "[PlanProductID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OrderServices_OrderID",
                table: "OrderServices",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderServices_PlanServiceID",
                table: "OrderServices",
                column: "PlanServiceID",
                unique: true,
                filter: "[PlanServiceID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OrderServices_ServiceID",
                table: "OrderServices",
                column: "ServiceID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_PlanProduct_PlanProductID",
                table: "OrderProducts",
                column: "PlanProductID",
                principalTable: "PlanProduct",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducts_PlanProduct_PlanProductID",
                table: "OrderProducts");

            migrationBuilder.DropTable(
                name: "OrderServices");

            migrationBuilder.DropIndex(
                name: "IX_OrderProducts_PlanProductID",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "LandPreparationDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "LastHarvestDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "SewingDate",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "PlanProductID",
                table: "OrderProducts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_PlanProductID",
                table: "OrderProducts",
                column: "PlanProductID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_PlanProduct_PlanProductID",
                table: "OrderProducts",
                column: "PlanProductID",
                principalTable: "PlanProduct",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
