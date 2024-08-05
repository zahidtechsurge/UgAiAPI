using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeTerritoryAssignments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeDistrictAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DitrictID = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeDistrictAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeDistrictAssignments_District_DitrictID",
                        column: x => x.DitrictID,
                        principalTable: "District",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeDistrictAssignments_tblFarmers_UserID",
                        column: x => x.UserID,
                        principalTable: "tblFarmers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeRegionAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RegionID = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    tblDistrictID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeRegionAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeRegionAssignments_District_tblDistrictID",
                        column: x => x.tblDistrictID,
                        principalTable: "District",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_EmployeeRegionAssignments_Regions_RegionID",
                        column: x => x.RegionID,
                        principalTable: "Regions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeRegionAssignments_tblFarmers_UserID",
                        column: x => x.UserID,
                        principalTable: "tblFarmers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDistrictAssignments_DitrictID",
                table: "EmployeeDistrictAssignments",
                column: "DitrictID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDistrictAssignments_UserID",
                table: "EmployeeDistrictAssignments",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRegionAssignments_RegionID",
                table: "EmployeeRegionAssignments",
                column: "RegionID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRegionAssignments_tblDistrictID",
                table: "EmployeeRegionAssignments",
                column: "tblDistrictID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRegionAssignments_UserID",
                table: "EmployeeRegionAssignments",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeDistrictAssignments");

            migrationBuilder.DropTable(
                name: "EmployeeRegionAssignments");
        }
    }
}
