using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addColumn_PlanChangeStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "PlanService",
                newName: "SewingDate");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "PlanService",
                newName: "LastHarvestDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "LandPreparationDate",
                table: "PlanService",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "PlanChangeStatus",
                table: "Plan",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LandPreparationDate",
                table: "PlanService");

            migrationBuilder.DropColumn(
                name: "PlanChangeStatus",
                table: "Plan");

            migrationBuilder.RenameColumn(
                name: "SewingDate",
                table: "PlanService",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "LastHarvestDate",
                table: "PlanService",
                newName: "EndDate");
        }
    }
}
