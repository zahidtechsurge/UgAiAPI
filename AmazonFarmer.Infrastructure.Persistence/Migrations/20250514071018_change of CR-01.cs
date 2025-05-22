using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class changeofCR01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ModeOfPayment",
                table: "Plan",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ConfigurationTable",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModeOfPayment",
                table: "Plan");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ConfigurationTable");
        }
    }
}
