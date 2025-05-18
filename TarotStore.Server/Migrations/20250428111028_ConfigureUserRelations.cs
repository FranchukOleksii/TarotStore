using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TarotStore.Server.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureUserRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_UserId",
                table: "UserDetails",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserByRole_UserId",
                table: "UserByRole",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserByRole_User_UserId",
                table: "UserByRole",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDetails_User_UserId",
                table: "UserDetails",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserByRole_User_UserId",
                table: "UserByRole");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDetails_User_UserId",
                table: "UserDetails");

            migrationBuilder.DropIndex(
                name: "IX_UserDetails_UserId",
                table: "UserDetails");

            migrationBuilder.DropIndex(
                name: "IX_UserByRole_UserId",
                table: "UserByRole");
        }
    }
}
