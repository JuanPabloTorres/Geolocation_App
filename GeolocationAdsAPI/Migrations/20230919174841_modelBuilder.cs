using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeolocationAdsAPI.Migrations
{
    /// <inheritdoc />
    public partial class modelBuilder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdvertisementSettings_Advertisements_AdvertisementId",
                table: "AdvertisementSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdvertisementSettings",
                table: "AdvertisementSettings");

            migrationBuilder.RenameColumn(
                name: "AdvertisementId",
                table: "AdvertisementSettings",
                newName: "AdvertisementID");

            migrationBuilder.RenameIndex(
                name: "IX_AdvertisementSettings_AdvertisementId",
                table: "AdvertisementSettings",
                newName: "IX_AdvertisementSettings_AdvertisementID");

            migrationBuilder.AlterColumn<int>(
                name: "AdvertisementID",
                table: "AdvertisementSettings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            //migrationBuilder.AddColumn<int>(
            //    name: "AdvertisementId",
            //    table: "AdvertisementSettings",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<int>(
            //    name: "AdvertisementID1",
            //    table: "AdvertisementSettings",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_AdvertisementSettings",
            //    table: "AdvertisementSettings",
            //    columns: new[] { "AdvertisementId", "SettingId" });

            //migrationBuilder.AddForeignKey(
            //    name: "FK_AdvertisementSettings_Advertisements_AdvertisementID",
            //    table: "AdvertisementSettings",
            //    column: "AdvertisementID",
            //    principalTable: "Advertisements",
            //    principalColumn: "ID");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_AdvertisementSettings_Advertisements_AdvertisementId",
            //    table: "AdvertisementSettings",
            //    column: "AdvertisementId",
            //    principalTable: "Advertisements",
            //    principalColumn: "ID",
            //    onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdvertisementSettings_Advertisements_AdvertisementID",
                table: "AdvertisementSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_AdvertisementSettings_Advertisements_AdvertisementId",
                table: "AdvertisementSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdvertisementSettings",
                table: "AdvertisementSettings");

            migrationBuilder.DropColumn(
                name: "AdvertisementId",
                table: "AdvertisementSettings");

            migrationBuilder.DropColumn(
                name: "AdvertisementID1",
                table: "AdvertisementSettings");

            migrationBuilder.RenameColumn(
                name: "AdvertisementID",
                table: "AdvertisementSettings",
                newName: "AdvertisementId");

            migrationBuilder.RenameIndex(
                name: "IX_AdvertisementSettings_AdvertisementID",
                table: "AdvertisementSettings",
                newName: "IX_AdvertisementSettings_AdvertisementId");

            migrationBuilder.AlterColumn<int>(
                name: "AdvertisementId",
                table: "AdvertisementSettings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdvertisementSettings",
                table: "AdvertisementSettings",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvertisementSettings_Advertisements_AdvertisementId",
                table: "AdvertisementSettings",
                column: "AdvertisementId",
                principalTable: "Advertisements",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
