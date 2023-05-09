using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gharbetti.Migrations
{
    /// <inheritdoc />
    public partial class addBindingInFloorDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                table: "Complains",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Complains_TenantId",
                table: "Complains",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Complains_AspNetUsers_TenantId",
                table: "Complains",
                column: "TenantId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complains_AspNetUsers_TenantId",
                table: "Complains");

            migrationBuilder.DropIndex(
                name: "IX_Complains_TenantId",
                table: "Complains");

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                table: "Complains",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
