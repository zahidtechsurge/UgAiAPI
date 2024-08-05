using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addPDFPathOnAuthorityLetterPDFGUID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "UnitOfMeasureSales",
                table: "tblUnitOfMeasures",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitofMeasureReporting",
                table: "tblUnitOfMeasures",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PDFGUID",
                table: "AuthorityLetters",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitOfMeasureSales",
                table: "tblUnitOfMeasures");

            migrationBuilder.DropColumn(
                name: "UnitofMeasureReporting",
                table: "tblUnitOfMeasures");

            migrationBuilder.DropColumn(
                name: "PDFGUID",
                table: "AuthorityLetters");
        }
    }
}
