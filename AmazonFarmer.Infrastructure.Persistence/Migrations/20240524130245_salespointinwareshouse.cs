﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class salespointinwareshouse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SalePoint",
                table: "Warehouse",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalePoint",
                table: "Warehouse");
        }
    }
}