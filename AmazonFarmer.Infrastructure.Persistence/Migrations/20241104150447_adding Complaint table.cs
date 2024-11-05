using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addingComplainttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Complaints",
                columns: table => new
                {
                    ComplaintID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComplaintTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComplaintDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComplaintStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedByID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Complaints", x => x.ComplaintID);
                    table.ForeignKey(
                        name: "FK_Complaints_tblFarmers_CreatedByID",
                        column: x => x.CreatedByID,
                        principalTable: "tblFarmers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_CreatedByID",
                table: "Complaints",
                column: "CreatedByID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Complaints");
        }
    }
}
