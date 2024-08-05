using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class added_FarmID_PlanID_OrderID_ColumnsDeviceNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FarmID",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OrderID",
                table: "Notifications",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlanID",
                table: "Notifications",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FarmID",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "OrderID",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "PlanID",
                table: "Notifications");
        }
    }
}
