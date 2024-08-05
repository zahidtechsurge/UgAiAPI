using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class tablefixesforrelationsMore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorityLetterDetails_Products_TblProductID",
                table: "AuthorityLetterDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_AuthorityLetters_Warehouse_tblwarehouseID",
                table: "AuthorityLetters");

            migrationBuilder.DropForeignKey(
                name: "FK_FarmChangeRequests_District_tblDistrictID",
                table: "FarmChangeRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_FarmChangeRequests_Farms_tblfarmFarmID",
                table: "FarmChangeRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Farms_District_tblDistrictID",
                table: "Farms");

            migrationBuilder.DropIndex(
                name: "IX_Farms_tblDistrictID",
                table: "Farms");

            migrationBuilder.DropIndex(
                name: "IX_FarmChangeRequests_tblDistrictID",
                table: "FarmChangeRequests");

            migrationBuilder.DropIndex(
                name: "IX_FarmChangeRequests_tblfarmFarmID",
                table: "FarmChangeRequests");

            migrationBuilder.DropIndex(
                name: "IX_AuthorityLetters_tblwarehouseID",
                table: "AuthorityLetters");

            migrationBuilder.DropIndex(
                name: "IX_AuthorityLetterDetails_TblProductID",
                table: "AuthorityLetterDetails");

            migrationBuilder.DropColumn(
                name: "tblDistrictID",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "tblDistrictID",
                table: "FarmChangeRequests");

            migrationBuilder.DropColumn(
                name: "tblfarmFarmID",
                table: "FarmChangeRequests");

            migrationBuilder.DropColumn(
                name: "tblwarehouseID",
                table: "AuthorityLetters");

            migrationBuilder.DropColumn(
                name: "TblProductID",
                table: "AuthorityLetterDetails");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "tblDistrictID",
                table: "Farms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "tblDistrictID",
                table: "FarmChangeRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "tblfarmFarmID",
                table: "FarmChangeRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "tblwarehouseID",
                table: "AuthorityLetters",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TblProductID",
                table: "AuthorityLetterDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Farms_tblDistrictID",
                table: "Farms",
                column: "tblDistrictID");

            migrationBuilder.CreateIndex(
                name: "IX_FarmChangeRequests_tblDistrictID",
                table: "FarmChangeRequests",
                column: "tblDistrictID");

            migrationBuilder.CreateIndex(
                name: "IX_FarmChangeRequests_tblfarmFarmID",
                table: "FarmChangeRequests",
                column: "tblfarmFarmID");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorityLetters_tblwarehouseID",
                table: "AuthorityLetters",
                column: "tblwarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorityLetterDetails_TblProductID",
                table: "AuthorityLetterDetails",
                column: "TblProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorityLetterDetails_Products_TblProductID",
                table: "AuthorityLetterDetails",
                column: "TblProductID",
                principalTable: "Products",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorityLetters_Warehouse_tblwarehouseID",
                table: "AuthorityLetters",
                column: "tblwarehouseID",
                principalTable: "Warehouse",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_FarmChangeRequests_District_tblDistrictID",
                table: "FarmChangeRequests",
                column: "tblDistrictID",
                principalTable: "District",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_FarmChangeRequests_Farms_tblfarmFarmID",
                table: "FarmChangeRequests",
                column: "tblfarmFarmID",
                principalTable: "Farms",
                principalColumn: "FarmID");

            migrationBuilder.AddForeignKey(
                name: "FK_Farms_District_tblDistrictID",
                table: "Farms",
                column: "tblDistrictID",
                principalTable: "District",
                principalColumn: "ID");
        }
    }
}
