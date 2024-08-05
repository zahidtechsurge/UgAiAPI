using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EmailNotificationTranslations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Body",
                table: "EmailNotifications");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "EmailNotifications");

            migrationBuilder.CreateTable(
                name: "EmailNotificationTranslations",
                columns: table => new
                {
                    RecID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SMSBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FCMBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NotificationID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailNotificationTranslations", x => x.RecID);
                    table.ForeignKey(
                        name: "FK_EmailNotificationTranslations_EmailNotifications_NotificationID",
                        column: x => x.NotificationID,
                        principalTable: "EmailNotifications",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmailNotificationTranslations_Languages_LanguageCode",
                        column: x => x.LanguageCode,
                        principalTable: "Languages",
                        principalColumn: "LanguageCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailNotificationTranslations_LanguageCode",
                table: "EmailNotificationTranslations",
                column: "LanguageCode");

            migrationBuilder.CreateIndex(
                name: "IX_EmailNotificationTranslations_NotificationID",
                table: "EmailNotificationTranslations",
                column: "NotificationID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailNotificationTranslations");

            migrationBuilder.AddColumn<string>(
                name: "Body",
                table: "EmailNotifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "EmailNotifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
