using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addednewcolumnfortransactionconfirmationcount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TransactionStatusCheckAttempts",
                table: "Transactions",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionStatusCheckAttempts",
                table: "Transactions");
        }
    }
}
