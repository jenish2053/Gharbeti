using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gharbetti.Migrations
{
    /// <inheritdoc />
    public partial class changeHouseIdToMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Message_HouseId",
                table: "Message",
                column: "HouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Houses_HouseId",
                table: "Message",
                column: "HouseId",
                principalTable: "Houses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Houses_HouseId",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_HouseId",
                table: "Message");
        }
    }
}
