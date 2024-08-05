using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class OneLinkIntegrationUpdates2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Crops_CropID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Plan_PlanID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Plan_tblPlanID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Warehouse_WarehouseID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_tblFarmers_CreatedByID",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CreatedByID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CropID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PlanID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_tblPlanID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_WarehouseID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "tblPlanID",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByID",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CreatedByID",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "OrderID",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "tblPlanID",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CreatedByID",
                table: "Orders",
                column: "CreatedByID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CropID",
                table: "Orders",
                column: "CropID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PlanID",
                table: "Orders",
                column: "PlanID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_tblPlanID",
                table: "Orders",
                column: "tblPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_WarehouseID",
                table: "Orders",
                column: "WarehouseID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Crops_CropID",
                table: "Orders",
                column: "CropID",
                principalTable: "Crops",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Plan_PlanID",
                table: "Orders",
                column: "PlanID",
                principalTable: "Plan",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Plan_tblPlanID",
                table: "Orders",
                column: "tblPlanID",
                principalTable: "Plan",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Warehouse_WarehouseID",
                table: "Orders",
                column: "WarehouseID",
                principalTable: "Warehouse",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_tblFarmers_CreatedByID",
                table: "Orders",
                column: "CreatedByID",
                principalTable: "tblFarmers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
