using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeolocationAdsAPI.Migrations
{
    /// <inheritdoc />
    public partial class navigationpropertygeoAd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Advertisements_GeolocationAds_GeolocationAdId",
            //    table: "Advertisements");

            migrationBuilder.AddColumn<int>(
                name: "GeolocationAdId",
                table: "Advertisements",
                type: "int",
                nullable: true

              );

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_GeolocationAds_GeolocationAdId",
                table: "Advertisements",
                column: "GeolocationAdId",
                principalTable: "GeolocationAds",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_GeolocationAds_GeolocationAdId",
                table: "Advertisements");

            migrationBuilder.AddColumn<int>(
                name: "GeolocationAdId",
                table: "Advertisements",
                type: "int",
                nullable: false,
                defaultValue: 0


               );

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_GeolocationAds_GeolocationAdId",
                table: "Advertisements",
                column: "GeolocationAdId",
                principalTable: "GeolocationAds",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
