using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeolocationAdsAPI.Migrations
{
    /// <inheritdoc />
    public partial class CaptureModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CaptureID",
                table: "Advertisements",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Captures",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    UpdateBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Captures", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_CaptureID",
                table: "Advertisements",
                column: "CaptureID");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_Captures_CaptureID",
                table: "Advertisements",
                column: "CaptureID",
                principalTable: "Captures",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_Captures_CaptureID",
                table: "Advertisements");

            migrationBuilder.DropTable(
                name: "Captures");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_CaptureID",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "CaptureID",
                table: "Advertisements");
        }
    }
}
