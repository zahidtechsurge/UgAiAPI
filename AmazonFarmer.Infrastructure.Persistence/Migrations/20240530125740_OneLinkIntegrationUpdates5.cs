using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class OneLinkIntegrationUpdates5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_BillingPaymentRequest_OrderID",
                table: "BillingPaymentRequest",
                column: "OrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_BillingPaymentRequest_Orders_OrderID",
                table: "BillingPaymentRequest",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "OrderID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillingPaymentRequest_Orders_OrderID",
                table: "BillingPaymentRequest");

            migrationBuilder.DropIndex(
                name: "IX_BillingPaymentRequest_OrderID",
                table: "BillingPaymentRequest");
        }
    }
}
