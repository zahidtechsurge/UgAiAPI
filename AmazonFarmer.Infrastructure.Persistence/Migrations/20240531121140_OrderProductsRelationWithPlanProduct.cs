using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class OrderProductsRelationWithPlanProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlanProductID",
                table: "OrderProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducts_PlanProduct_PlanProductID",
                table: "OrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_OrderProducts_PlanProductID",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "PlanProductID",
                table: "OrderProducts");
        }
    }
}
