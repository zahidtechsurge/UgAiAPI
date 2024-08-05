using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TblRegion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Warehouse",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DistrictID",
                table: "Warehouse",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WHCode",
                table: "Warehouse",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TehsilCode",
                table: "Tehsils",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RegionId",
                table: "District",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CityCode",
                table: "Cities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegionCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Warehouse_DistrictID",
                table: "Warehouse",
                column: "DistrictID");

            migrationBuilder.CreateIndex(
                name: "IX_District_RegionId",
                table: "District",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_District_Regions_RegionId",
                table: "District",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouse_District_DistrictID",
                table: "Warehouse",
                column: "DistrictID",
                principalTable: "District",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_District_Regions_RegionId",
                table: "District");

            migrationBuilder.DropForeignKey(
                name: "FK_Warehouse_District_DistrictID",
                table: "Warehouse");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Warehouse_DistrictID",
                table: "Warehouse");

            migrationBuilder.DropIndex(
                name: "IX_District_RegionId",
                table: "District");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Warehouse");

            migrationBuilder.DropColumn(
                name: "DistrictID",
                table: "Warehouse");

            migrationBuilder.DropColumn(
                name: "WHCode",
                table: "Warehouse");

            migrationBuilder.DropColumn(
                name: "TehsilCode",
                table: "Tehsils");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "District");

            migrationBuilder.DropColumn(
                name: "CityCode",
                table: "Cities");
        }
    }
}
