using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeolocationAdsAPI.Migrations
{
    /// <inheritdoc />
    public partial class adContentName_Prop_To_ContModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentName",
                table: "ContentTypes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentName",
                table: "ContentTypes");
        }
    }
}
