using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class orderservices4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderServices_Orders_OrderID",
                table: "OrderServices");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderServices_PlanService_PlanServiceID",
                table: "OrderServices");

            migrationBuilder.DropIndex(
                name: "IX_OrderServices_OrderID",
                table: "OrderServices");

            migrationBuilder.DropIndex(
                name: "IX_OrderServices_PlanServiceID",
                table: "OrderServices");

            migrationBuilder.DropColumn(
                name: "OrderID",
                table: "OrderServices");

            migrationBuilder.RenameColumn(
                name: "PlanServiceID",
                table: "OrderServices",
                newName: "PlanID");

            migrationBuilder.AddColumn<int>(
                name: "CropID",
                table: "OrderServices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderServices_PlanID",
                table: "OrderServices",
                column: "PlanID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderServices_Plan_PlanID",
                table: "OrderServices",
                column: "PlanID",
                principalTable: "Plan",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderServices_Plan_PlanID",
                table: "OrderServices");

            migrationBuilder.DropIndex(
                name: "IX_OrderServices_PlanID",
                table: "OrderServices");

            migrationBuilder.DropColumn(
                name: "CropID",
                table: "OrderServices");

            migrationBuilder.RenameColumn(
                name: "PlanID",
                table: "OrderServices",
                newName: "PlanServiceID");

            migrationBuilder.AddColumn<long>(
                name: "OrderID",
                table: "OrderServices",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

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

            migrationBuilder.AddForeignKey(
                name: "FK_OrderServices_Orders_OrderID",
                table: "OrderServices",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "OrderID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderServices_PlanService_PlanServiceID",
                table: "OrderServices",
                column: "PlanServiceID",
                principalTable: "PlanService",
                principalColumn: "ID");
        }
    }
}
