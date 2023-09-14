using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeolocationAdsAPI.Migrations
{
    /// <inheritdoc />
    public partial class newModelContentType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.CreateTable(
                name: "ContentTypes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    AdvertisingId = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    UpdateBy = table.Column<int>(type: "int", nullable: false)
                },
    constraints: table =>
    {
        table.PrimaryKey("PK_ContentTypes", x => x.ID);
        table.ForeignKey(
            name: "FK_ContentTypes_Advertisements_AdvertisingId",
            column: x => x.AdvertisingId,
            principalTable: "Advertisements",
            principalColumn: "ID",
            onDelete: ReferentialAction.Cascade);
    });

            migrationBuilder.CreateIndex(
                name: "IX_ContentTypes_AdvertisingId",
                table: "ContentTypes",
                column: "AdvertisingId");


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
               name: "ContentTypes");
        }
    }
}
