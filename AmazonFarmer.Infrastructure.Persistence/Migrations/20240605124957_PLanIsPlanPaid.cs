using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PLanIsPlanPaid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPlanPaid",
                table: "Plan",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<long>(
                name: "ParentOrderID",
                table: "Orders",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ParentOrderID",
                table: "Orders",
                column: "ParentOrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Orders_ParentOrderID",
                table: "Orders",
                column: "ParentOrderID",
                principalTable: "Orders",
                principalColumn: "OrderID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Orders_ParentOrderID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ParentOrderID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IsPlanPaid",
                table: "Plan");

            migrationBuilder.AlterColumn<int>(
                name: "ParentOrderID",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
