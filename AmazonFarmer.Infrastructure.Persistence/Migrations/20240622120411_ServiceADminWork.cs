using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ServiceADminWork : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedDate",
                table: "OrderServices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "OrderServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RequestStatus",
                table: "OrderServices",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ScehduledDate",
                table: "OrderServices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VendorStatus",
                table: "OrderServices",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedDate",
                table: "OrderServices");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "OrderServices");

            migrationBuilder.DropColumn(
                name: "RequestStatus",
                table: "OrderServices");

            migrationBuilder.DropColumn(
                name: "ScehduledDate",
                table: "OrderServices");

            migrationBuilder.DropColumn(
                name: "VendorStatus",
                table: "OrderServices");
        }
    }
}
