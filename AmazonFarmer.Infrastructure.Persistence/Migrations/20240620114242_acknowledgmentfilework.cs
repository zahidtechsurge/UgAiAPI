using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class acknowledgmentfilework : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "PaymentAcknowledgments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Amount",
                table: "PaymentAcknowledgments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "PaymentAcknowledgments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "PaymentAcknowledgments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "PaymentAcknowledgments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DatePaid",
                table: "PaymentAcknowledgments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "FileID",
                table: "PaymentAcknowledgments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MaskedConsumerNumber",
                table: "PaymentAcknowledgments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentMode",
                table: "PaymentAcknowledgments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "STAN",
                table: "PaymentAcknowledgments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SettlementDate",
                table: "PaymentAcknowledgments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TimePaid",
                table: "PaymentAcknowledgments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Trans_Auth_ID",
                table: "PaymentAcknowledgments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "PaymentAcknowledgmentFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateReceived = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RowsCount = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentAcknowledgmentFiles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentAcknowledgments_FileID",
                table: "PaymentAcknowledgments",
                column: "FileID");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentAcknowledgments_PaymentAcknowledgmentFiles_FileID",
                table: "PaymentAcknowledgments",
                column: "FileID",
                principalTable: "PaymentAcknowledgmentFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentAcknowledgments_PaymentAcknowledgmentFiles_FileID",
                table: "PaymentAcknowledgments");

            migrationBuilder.DropTable(
                name: "PaymentAcknowledgmentFiles");

            migrationBuilder.DropIndex(
                name: "IX_PaymentAcknowledgments_FileID",
                table: "PaymentAcknowledgments");

            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "PaymentAcknowledgments");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "PaymentAcknowledgments");

            migrationBuilder.DropColumn(
                name: "BankName",
                table: "PaymentAcknowledgments");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "PaymentAcknowledgments");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "PaymentAcknowledgments");

            migrationBuilder.DropColumn(
                name: "DatePaid",
                table: "PaymentAcknowledgments");

            migrationBuilder.DropColumn(
                name: "FileID",
                table: "PaymentAcknowledgments");

            migrationBuilder.DropColumn(
                name: "MaskedConsumerNumber",
                table: "PaymentAcknowledgments");

            migrationBuilder.DropColumn(
                name: "PaymentMode",
                table: "PaymentAcknowledgments");

            migrationBuilder.DropColumn(
                name: "STAN",
                table: "PaymentAcknowledgments");

            migrationBuilder.DropColumn(
                name: "SettlementDate",
                table: "PaymentAcknowledgments");

            migrationBuilder.DropColumn(
                name: "TimePaid",
                table: "PaymentAcknowledgments");

            migrationBuilder.DropColumn(
                name: "Trans_Auth_ID",
                table: "PaymentAcknowledgments");
        }
    }
}
