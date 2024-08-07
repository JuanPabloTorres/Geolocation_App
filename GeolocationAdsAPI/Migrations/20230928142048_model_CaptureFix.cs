using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeolocationAdsAPI.Migrations
{
    /// <inheritdoc />
    public partial class model_CaptureFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Captures_Advertisements_AdvertisementsID",
            //    table: "Captures");

            //migrationBuilder.DropIndex(
            //    name: "IX_Captures_AdvertisementsID",
            //    table: "Captures");

            //migrationBuilder.DropColumn(
            //    name: "AdvertisementsID",
            //    table: "Captures");

            migrationBuilder.AddColumn<int>(
                name: "AdvertisementId",
                table: "Captures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Captures_AdvertisementId",
                table: "Captures",
                column: "AdvertisementId");

            migrationBuilder.AddForeignKey(
                name: "FK_Captures_Advertisements_AdvertisementId",
                table: "Captures",
                column: "AdvertisementId",
                principalTable: "Advertisements",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Captures_Advertisements_AdvertisementId",
            //    table: "Captures");

            //migrationBuilder.DropIndex(
            //    name: "IX_Captures_AdvertisementId",
            //    table: "Captures");

            //migrationBuilder.DropColumn(
            //    name: "AdvertisementId",
            //    table: "Captures");

            //migrationBuilder.AddColumn<int>(
            //    name: "AdvertisementsID",
            //    table: "Captures",
            //    type: "int",
            //    nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Captures_AdvertisementsID",
                table: "Captures",
                column: "AdvertisementsID");

            migrationBuilder.AddForeignKey(
                name: "FK_Captures_Advertisements_AdvertisementsID",
                table: "Captures",
                column: "AdvertisementsID",
                principalTable: "Advertisements",
                principalColumn: "ID");
        }
    }
}
