using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class multicropsTalha : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Crops_CropID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanCrop_Crops_CropID",
                table: "PlanCrop");

            migrationBuilder.AlterColumn<int>(
                name: "CropID",
                table: "PlanCrop",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CropGroupID",
                table: "PlanCrop",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "tblCropGroupCropsID",
                table: "PlanCrop",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CropGroupID",
                table: "OrderServices",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "tblCropID",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CropGroup",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CropGroup", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CropGroupCrops",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CropGroupID = table.Column<int>(type: "int", nullable: false),
                    CropID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CropGroupCrops", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CropGroupCrops_CropGroup_CropGroupID",
                        column: x => x.CropGroupID,
                        principalTable: "CropGroup",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CropGroupCrops_Crops_CropID",
                        column: x => x.CropID,
                        principalTable: "Crops",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanCrop_CropGroupID",
                table: "PlanCrop",
                column: "CropGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanCrop_tblCropGroupCropsID",
                table: "PlanCrop",
                column: "tblCropGroupCropsID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_tblCropID",
                table: "Orders",
                column: "tblCropID");

            migrationBuilder.CreateIndex(
                name: "IX_CropGroupCrops_CropGroupID",
                table: "CropGroupCrops",
                column: "CropGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_CropGroupCrops_CropID",
                table: "CropGroupCrops",
                column: "CropID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CropGroup_CropID",
                table: "Orders",
                column: "CropID",
                principalTable: "CropGroup",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Crops_tblCropID",
                table: "Orders",
                column: "tblCropID",
                principalTable: "Crops",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanCrop_CropGroupCrops_tblCropGroupCropsID",
                table: "PlanCrop",
                column: "tblCropGroupCropsID",
                principalTable: "CropGroupCrops",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanCrop_CropGroup_CropGroupID",
                table: "PlanCrop",
                column: "CropGroupID",
                principalTable: "CropGroup",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanCrop_Crops_CropID",
                table: "PlanCrop",
                column: "CropID",
                principalTable: "Crops",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CropGroup_CropID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Crops_tblCropID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanCrop_CropGroupCrops_tblCropGroupCropsID",
                table: "PlanCrop");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanCrop_CropGroup_CropGroupID",
                table: "PlanCrop");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanCrop_Crops_CropID",
                table: "PlanCrop");

            migrationBuilder.DropTable(
                name: "CropGroupCrops");

            migrationBuilder.DropTable(
                name: "CropGroup");

            migrationBuilder.DropIndex(
                name: "IX_PlanCrop_CropGroupID",
                table: "PlanCrop");

            migrationBuilder.DropIndex(
                name: "IX_PlanCrop_tblCropGroupCropsID",
                table: "PlanCrop");

            migrationBuilder.DropIndex(
                name: "IX_Orders_tblCropID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CropGroupID",
                table: "PlanCrop");

            migrationBuilder.DropColumn(
                name: "tblCropGroupCropsID",
                table: "PlanCrop");

            migrationBuilder.DropColumn(
                name: "CropGroupID",
                table: "OrderServices");

            migrationBuilder.DropColumn(
                name: "tblCropID",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "CropID",
                table: "PlanCrop",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Crops_CropID",
                table: "Orders",
                column: "CropID",
                principalTable: "Crops",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanCrop_Crops_CropID",
                table: "PlanCrop",
                column: "CropID",
                principalTable: "Crops",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
