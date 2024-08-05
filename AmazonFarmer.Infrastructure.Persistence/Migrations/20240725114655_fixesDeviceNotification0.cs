using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class fixesDeviceNotification0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_DeviceNotification_DeviceNotificationID",
                table: "Notifications");

            migrationBuilder.AddColumn<string>(
                name: "ShipToPartyCode",
                table: "tblFarmers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShipToPartyName",
                table: "tblFarmers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NotificationRequestStatus",
                table: "Notifications",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AuthorityLetterID",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthorityLetterNo",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConsumerNumber",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FarmApplicationID",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FarmName",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoogleMapLinkWithCoordinated",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewPaymentDueDate",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PKRAmount",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PickUPDate",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReasonCommentBox",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReasonsDropdownID",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WarehouseID",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ReasonsDropdownID",
                table: "Notifications",
                column: "ReasonsDropdownID");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_WarehouseID",
                table: "Notifications",
                column: "WarehouseID");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_EmailNotifications_DeviceNotificationID",
                table: "Notifications",
                column: "DeviceNotificationID",
                principalTable: "EmailNotifications",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Reasons_ReasonsDropdownID",
                table: "Notifications",
                column: "ReasonsDropdownID",
                principalTable: "Reasons",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Warehouse_WarehouseID",
                table: "Notifications",
                column: "WarehouseID",
                principalTable: "Warehouse",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_EmailNotifications_DeviceNotificationID",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Reasons_ReasonsDropdownID",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Warehouse_WarehouseID",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_ReasonsDropdownID",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_WarehouseID",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ShipToPartyCode",
                table: "tblFarmers");

            migrationBuilder.DropColumn(
                name: "ShipToPartyName",
                table: "tblFarmers");

            migrationBuilder.DropColumn(
                name: "AuthorityLetterID",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "AuthorityLetterNo",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ConsumerNumber",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "FarmApplicationID",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "FarmName",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "GoogleMapLinkWithCoordinated",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "NewPaymentDueDate",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "PKRAmount",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "PickUPDate",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ReasonCommentBox",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ReasonsDropdownID",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "WarehouseID",
                table: "Notifications");

            migrationBuilder.AlterColumn<int>(
                name: "NotificationRequestStatus",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_DeviceNotification_DeviceNotificationID",
                table: "Notifications",
                column: "DeviceNotificationID",
                principalTable: "DeviceNotification",
                principalColumn: "NotificationID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
