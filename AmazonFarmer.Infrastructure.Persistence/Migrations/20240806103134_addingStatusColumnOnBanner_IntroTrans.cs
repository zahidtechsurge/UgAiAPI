using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addingStatusColumnOnBanner_IntroTrans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LanguageCode",
                table: "WarehouseTranslation",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "IntroLanguages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "BannerLanguages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTranslation_LanguageCode",
                table: "WarehouseTranslation",
                column: "LanguageCode");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseTranslation_Languages_LanguageCode",
                table: "WarehouseTranslation",
                column: "LanguageCode",
                principalTable: "Languages",
                principalColumn: "LanguageCode",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseTranslation_Languages_LanguageCode",
                table: "WarehouseTranslation");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseTranslation_LanguageCode",
                table: "WarehouseTranslation");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "IntroLanguages");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "BannerLanguages");

            migrationBuilder.AlterColumn<string>(
                name: "LanguageCode",
                table: "WarehouseTranslation",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
