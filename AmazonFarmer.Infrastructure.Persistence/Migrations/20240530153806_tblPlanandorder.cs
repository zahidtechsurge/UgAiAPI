using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class tblPlanandorder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Plan_tblPlanID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_tblPlanID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "tblPlanID",
                table: "Orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "tblPlanID",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_tblPlanID",
                table: "Orders",
                column: "tblPlanID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Plan_tblPlanID",
                table: "Orders",
                column: "tblPlanID",
                principalTable: "Plan",
                principalColumn: "ID");
        }
    }
}
