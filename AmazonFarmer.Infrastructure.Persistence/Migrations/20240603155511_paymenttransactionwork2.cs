using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class paymenttransactionwork2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_BillingPaymentRequest_BillPaymentRequestID",
                table: "Transactions");

            migrationBuilder.AlterColumn<int>(
                name: "BillPaymentRequestID",
                table: "Transactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "TransactionLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionID = table.Column<int>(type: "int", nullable: false),
                    TransactionStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionLogs_Transactions_TransactionID",
                        column: x => x.TransactionID,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionLogs_TransactionID",
                table: "TransactionLogs",
                column: "TransactionID");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_BillingPaymentRequest_BillPaymentRequestID",
                table: "Transactions",
                column: "BillPaymentRequestID",
                principalTable: "BillingPaymentRequest",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_BillingPaymentRequest_BillPaymentRequestID",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "TransactionLogs");

            migrationBuilder.AlterColumn<int>(
                name: "BillPaymentRequestID",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_BillingPaymentRequest_BillPaymentRequestID",
                table: "Transactions",
                column: "BillPaymentRequestID",
                principalTable: "BillingPaymentRequest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
