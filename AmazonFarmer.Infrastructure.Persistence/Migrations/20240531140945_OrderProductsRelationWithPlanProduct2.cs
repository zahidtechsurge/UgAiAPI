using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class OrderProductsRelationWithPlanProduct2 : Migration
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

            migrationBuilder.AlterColumn<int>(
                name: "PlanProductID",
                table: "OrderProducts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_PlanProductID",
                table: "OrderProducts",
                column: "PlanProductID",
                unique: true,
                filter: "[PlanProductID] IS NOT NULL");

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

            migrationBuilder.DropIndex(
                name: "IX_OrderProducts_PlanProductID",
                table: "OrderProducts");

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
