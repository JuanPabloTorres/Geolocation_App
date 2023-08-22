using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeolocationAdsAPI.Migrations
{
    /// <inheritdoc />
    public partial class nullable_loginId_UserModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logins_Users_UserId",
                table: "Logins");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Logins_LoginId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Logins_UserId",
                table: "Logins");

            migrationBuilder.AlterColumn<int>(
                name: "LoginId",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Logins_LoginId",
                table: "Users",
                column: "LoginId",
                principalTable: "Logins",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Logins_LoginId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "LoginId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Logins_UserId",
                table: "Logins",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Logins_Users_UserId",
                table: "Logins",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Logins_LoginId",
                table: "Users",
                column: "LoginId",
                principalTable: "Logins",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
