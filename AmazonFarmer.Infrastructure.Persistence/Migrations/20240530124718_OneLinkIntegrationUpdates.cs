using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class OneLinkIntegrationUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorityLetters_Orders_OrderID",
                table: "AuthorityLetters");

            migrationBuilder.DropForeignKey(
                name: "FK_BillingPaymentRequest_Orders_OrderID",
                table: "BillingPaymentRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_BillingPaymentResponse_Orders_TblOrdersOrderID",
                table: "BillingPaymentResponse");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducts_Orders_OrderID",
                table: "OrderProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Orders_OrderID",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_OrderID",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_OrderProducts_OrderID",
                table: "OrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_BillingPaymentResponse_TblOrdersOrderID",
                table: "BillingPaymentResponse");

            migrationBuilder.DropIndex(
                name: "IX_BillingPaymentRequest_OrderID",
                table: "BillingPaymentRequest");

            migrationBuilder.DropIndex(
                name: "IX_AuthorityLetters_OrderID",
                table: "AuthorityLetters");

            migrationBuilder.DropColumn(
                name: "TblOrdersOrderID",
                table: "BillingPaymentResponse");

            migrationBuilder.AlterColumn<long>(
                name: "OrderID",
                table: "Transactions",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "OrderID",
                table: "OrderProducts",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "OrderID",
                table: "BillingPaymentRequest",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "OrderID",
                table: "BillingIquiryRequest",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "OrderID",
                table: "AuthorityLetters",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OrderID",
                table: "Transactions",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "OrderID",
                table: "OrderProducts",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "TblOrdersOrderID",
                table: "BillingPaymentResponse",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OrderID",
                table: "BillingPaymentRequest",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OrderID",
                table: "BillingIquiryRequest",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OrderID",
                table: "AuthorityLetters",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_OrderID",
                table: "Transactions",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_OrderID",
                table: "OrderProducts",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_BillingPaymentResponse_TblOrdersOrderID",
                table: "BillingPaymentResponse",
                column: "TblOrdersOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_BillingPaymentRequest_OrderID",
                table: "BillingPaymentRequest",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorityLetters_OrderID",
                table: "AuthorityLetters",
                column: "OrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorityLetters_Orders_OrderID",
                table: "AuthorityLetters",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "OrderID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BillingPaymentRequest_Orders_OrderID",
                table: "BillingPaymentRequest",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "OrderID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BillingPaymentResponse_Orders_TblOrdersOrderID",
                table: "BillingPaymentResponse",
                column: "TblOrdersOrderID",
                principalTable: "Orders",
                principalColumn: "OrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_Orders_OrderID",
                table: "OrderProducts",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "OrderID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Orders_OrderID",
                table: "Transactions",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "OrderID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
