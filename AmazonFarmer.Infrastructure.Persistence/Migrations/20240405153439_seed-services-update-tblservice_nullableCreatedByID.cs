using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonFarmer.Infrastructure.Persistence.Migrations
{
    public partial class seedservicesupdatetblservice_nullableCreatedByID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Service_tblFarmers_CreatedByID",
                table: "Service");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByID",
                table: "Service",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "BannerLanguages",
                keyColumn: "ID",
                keyValue: 1,
                column: "Image",
                value: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS9cD-r13pJWK--ScPagu2uFM7UQ0yjPpRhEktsO0e5vRb0KeULOexclEBcw2qq4YkyYjI&usqp=CAU");

            migrationBuilder.UpdateData(
                table: "BannerLanguages",
                keyColumn: "ID",
                keyValue: 2,
                column: "Image",
                value: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS9cD-r13pJWK--ScPagu2uFM7UQ0yjPpRhEktsO0e5vRb0KeULOexclEBcw2qq4YkyYjI&usqp=CAU");

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6145), new DateTime(2024, 5, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6148) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6154), new DateTime(2024, 5, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6156) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 3,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6160), new DateTime(2024, 5, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6161) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 4,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6165), new DateTime(2024, 5, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6167) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 5,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6170), new DateTime(2024, 5, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6172) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 6,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6176), new DateTime(2024, 5, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6177) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 7,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6181), new DateTime(2024, 5, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6182) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 8,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6186), new DateTime(2024, 5, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6188) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 9,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6191), new DateTime(2024, 5, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6193) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 10,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6196), new DateTime(2024, 5, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6198) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 11,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6202), new DateTime(2024, 5, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6203) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 12,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6207), new DateTime(2024, 5, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6208) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 13,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6212), new DateTime(2024, 5, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6232) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 14,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6260), new DateTime(2024, 5, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6262) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 15,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6266), new DateTime(2024, 5, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6268) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 16,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6272), new DateTime(2024, 5, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6273) });

            migrationBuilder.UpdateData(
                table: "IntroLanguages",
                keyColumn: "ID",
                keyValue: 1,
                column: "Image",
                value: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS9cD-r13pJWK--ScPagu2uFM7UQ0yjPpRhEktsO0e5vRb0KeULOexclEBcw2qq4YkyYjI&usqp=CAU");

            migrationBuilder.UpdateData(
                table: "IntroLanguages",
                keyColumn: "ID",
                keyValue: 2,
                column: "Image",
                value: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS9cD-r13pJWK--ScPagu2uFM7UQ0yjPpRhEktsO0e5vRb0KeULOexclEBcw2qq4YkyYjI&usqp=CAU");

            migrationBuilder.UpdateData(
                table: "IntroLanguages",
                keyColumn: "ID",
                keyValue: 3,
                column: "Image",
                value: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS9cD-r13pJWK--ScPagu2uFM7UQ0yjPpRhEktsO0e5vRb0KeULOexclEBcw2qq4YkyYjI&usqp=CAU");

            migrationBuilder.UpdateData(
                table: "IntroLanguages",
                keyColumn: "ID",
                keyValue: 4,
                column: "Image",
                value: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS9cD-r13pJWK--ScPagu2uFM7UQ0yjPpRhEktsO0e5vRb0KeULOexclEBcw2qq4YkyYjI&usqp=CAU");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6433));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6440));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6444));

            migrationBuilder.UpdateData(
                table: "Season",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 5, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(5836), new DateTime(2024, 4, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(5815) });

            migrationBuilder.UpdateData(
                table: "Season",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 5, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(5855), new DateTime(2024, 4, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(5854) });

            migrationBuilder.InsertData(
                table: "Service",
                columns: new[] { "ID", "Active", "CreatedByID", "CreatedDate", "Name" },
                values: new object[,]
                {
                    { 1, 1, null, new DateTime(2024, 4, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6535), "Soil Sampling" },
                    { 2, 1, null, new DateTime(2024, 4, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6540), "Geofencing" },
                    { 3, 1, null, new DateTime(2024, 4, 5, 20, 34, 16, 969, DateTimeKind.Local).AddTicks(6543), "Drone Footage" }
                });

            migrationBuilder.InsertData(
                table: "ServiceTranslation",
                columns: new[] { "ID", "Image", "LanguageCode", "ServiceID", "Text" },
                values: new object[,]
                {
                    { 1, "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png", "EN", 1, "Soil Sampling" },
                    { 2, "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png", "UR", 1, "مٹی کے نمونے لینے" },
                    { 3, "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png", "EN", 2, "Geofencing" },
                    { 4, "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png", "UR", 2, "جیوفینسنگ" },
                    { 5, "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png", "EN", 3, "Drone Footage" },
                    { 6, "https://w7.pngwing.com/pngs/531/598/png-transparent-computer-icons-crop-agriculture-farmer-grain-miscellaneous-food-leaf.png", "UR", 3, "ڈرون فوٹیج" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Service_tblFarmers_CreatedByID",
                table: "Service",
                column: "CreatedByID",
                principalTable: "tblFarmers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Service_tblFarmers_CreatedByID",
                table: "Service");

            migrationBuilder.DeleteData(
                table: "ServiceTranslation",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ServiceTranslation",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ServiceTranslation",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ServiceTranslation",
                keyColumn: "ID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ServiceTranslation",
                keyColumn: "ID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ServiceTranslation",
                keyColumn: "ID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Service",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Service",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Service",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByID",
                table: "Service",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "BannerLanguages",
                keyColumn: "ID",
                keyValue: 1,
                column: "Image",
                value: "/attachments/banner-01.JPG");

            migrationBuilder.UpdateData(
                table: "BannerLanguages",
                keyColumn: "ID",
                keyValue: 2,
                column: "Image",
                value: "/attachments/banner-01.JPG");

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6833), new DateTime(2024, 5, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6835) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6839), new DateTime(2024, 5, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6840) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 3,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6843), new DateTime(2024, 5, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6844) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 4,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6846), new DateTime(2024, 5, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6847) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 5,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6850), new DateTime(2024, 5, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6851) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 6,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6853), new DateTime(2024, 5, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6854) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 7,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6857), new DateTime(2024, 5, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6858) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 8,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6860), new DateTime(2024, 5, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6861) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 9,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6864), new DateTime(2024, 5, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6865) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 10,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6867), new DateTime(2024, 5, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6868) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 11,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6871), new DateTime(2024, 5, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6872) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 12,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6882), new DateTime(2024, 5, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6884) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 13,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6886), new DateTime(2024, 5, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6903) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 14,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6922), new DateTime(2024, 5, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6923) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 15,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6925), new DateTime(2024, 5, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6926) });

            migrationBuilder.UpdateData(
                table: "CropTimings",
                keyColumn: "ID",
                keyValue: 16,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(7002), new DateTime(2024, 5, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(7004) });

            migrationBuilder.UpdateData(
                table: "IntroLanguages",
                keyColumn: "ID",
                keyValue: 1,
                column: "Image",
                value: "/attachments/banner-01.JPG");

            migrationBuilder.UpdateData(
                table: "IntroLanguages",
                keyColumn: "ID",
                keyValue: 2,
                column: "Image",
                value: "/attachments/banner-01.JPG");

            migrationBuilder.UpdateData(
                table: "IntroLanguages",
                keyColumn: "ID",
                keyValue: 3,
                column: "Image",
                value: "/attachments/banner-02.JPG");

            migrationBuilder.UpdateData(
                table: "IntroLanguages",
                keyColumn: "ID",
                keyValue: 4,
                column: "Image",
                value: "/attachments/banner-02.JPG");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(7129));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(7133));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ID",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(7136));

            migrationBuilder.UpdateData(
                table: "Season",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 5, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6624), new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6610) });

            migrationBuilder.UpdateData(
                table: "Season",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 5, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6634), new DateTime(2024, 4, 4, 22, 12, 58, 532, DateTimeKind.Local).AddTicks(6633) });

            migrationBuilder.AddForeignKey(
                name: "FK_Service_tblFarmers_CreatedByID",
                table: "Service",
                column: "CreatedByID",
                principalTable: "tblFarmers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
