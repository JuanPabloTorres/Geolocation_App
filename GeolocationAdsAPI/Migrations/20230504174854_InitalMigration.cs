using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeolocationAdsAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitalMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Advertisements",
            //    columns: table => new
            //    {
            //        ID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Content = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
            //        Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        CreateBy = table.Column<int>(type: "int", nullable: false),
            //        UpdateBy = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Advertisements", x => x.ID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "GeolocationAds",
            //    columns: table => new
            //    {
            //        ID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        AdvertisingId = table.Column<int>(type: "int", nullable: false),
            //        Latitude = table.Column<double>(type: "float", nullable: false),
            //        Longitude = table.Column<double>(type: "float", nullable: false),
            //        CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        CreateBy = table.Column<int>(type: "int", nullable: false),
            //        UpdateBy = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_GeolocationAds", x => x.ID);
            //        table.ForeignKey(
            //            name: "FK_GeolocationAds_Advertisements_AdvertisingId",
            //            column: x => x.AdvertisingId,
            //            principalTable: "Advertisements",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_GeolocationAds_AdvertisingId",
            //    table: "GeolocationAds",
            //    column: "AdvertisingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeolocationAds");

            migrationBuilder.DropTable(
                name: "Advertisements");
        }
    }
}
