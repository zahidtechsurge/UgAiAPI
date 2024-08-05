using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class multicropsTalha2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanCrop_CropGroupCrops_tblCropGroupCropsID",
                table: "PlanCrop");

            migrationBuilder.DropIndex(
                name: "IX_PlanCrop_tblCropGroupCropsID",
                table: "PlanCrop");

            migrationBuilder.DropColumn(
                name: "tblCropGroupCropsID",
                table: "PlanCrop");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "tblCropGroupCropsID",
                table: "PlanCrop",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlanCrop_tblCropGroupCropsID",
                table: "PlanCrop",
                column: "tblCropGroupCropsID");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanCrop_CropGroupCrops_tblCropGroupCropsID",
                table: "PlanCrop",
                column: "tblCropGroupCropsID",
                principalTable: "CropGroupCrops",
                principalColumn: "ID");
        }
    }
}
