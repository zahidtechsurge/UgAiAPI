using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class tablefixesforrelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FarmChangeRequests_Tehsils_tblTehsilID",
                table: "FarmChangeRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Farms_Tehsils_tblTehsilID",
                table: "Farms");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileChangeRequests_Tehsils_tblTehsilID",
                table: "ProfileChangeRequests");

            migrationBuilder.DropIndex(
                name: "IX_ProfileChangeRequests_tblTehsilID",
                table: "ProfileChangeRequests");

            migrationBuilder.DropIndex(
                name: "IX_Farms_tblTehsilID",
                table: "Farms");

            migrationBuilder.DropIndex(
                name: "IX_FarmChangeRequests_tblTehsilID",
                table: "FarmChangeRequests");

            migrationBuilder.DropColumn(
                name: "tblTehsilID",
                table: "ProfileChangeRequests");

            migrationBuilder.DropColumn(
                name: "tblTehsilID",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "tblTehsilID",
                table: "FarmChangeRequests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "tblTehsilID",
                table: "ProfileChangeRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "tblTehsilID",
                table: "Farms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "tblTehsilID",
                table: "FarmChangeRequests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProfileChangeRequests_tblTehsilID",
                table: "ProfileChangeRequests",
                column: "tblTehsilID");

            migrationBuilder.CreateIndex(
                name: "IX_Farms_tblTehsilID",
                table: "Farms",
                column: "tblTehsilID");

            migrationBuilder.CreateIndex(
                name: "IX_FarmChangeRequests_tblTehsilID",
                table: "FarmChangeRequests",
                column: "tblTehsilID");

            migrationBuilder.AddForeignKey(
                name: "FK_FarmChangeRequests_Tehsils_tblTehsilID",
                table: "FarmChangeRequests",
                column: "tblTehsilID",
                principalTable: "Tehsils",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Farms_Tehsils_tblTehsilID",
                table: "Farms",
                column: "tblTehsilID",
                principalTable: "Tehsils",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileChangeRequests_Tehsils_tblTehsilID",
                table: "ProfileChangeRequests",
                column: "tblTehsilID",
                principalTable: "Tehsils",
                principalColumn: "ID");
        }
    }
}
