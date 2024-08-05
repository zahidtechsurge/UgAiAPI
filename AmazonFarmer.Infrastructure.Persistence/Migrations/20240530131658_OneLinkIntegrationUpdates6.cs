using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class OneLinkIntegrationUpdates6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_BillingIquiryRequest_OrderID",
                table: "BillingIquiryRequest",
                column: "OrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_BillingIquiryRequest_Orders_OrderID",
                table: "BillingIquiryRequest",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "OrderID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillingIquiryRequest_Orders_OrderID",
                table: "BillingIquiryRequest");

            migrationBuilder.DropIndex(
                name: "IX_BillingIquiryRequest_OrderID",
                table: "BillingIquiryRequest");
        }
    }
}
