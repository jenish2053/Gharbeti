using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gharbetti.Migrations
{
    /// <inheritdoc />
    public partial class addTenet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Tennets",
                table: "Tennets");

            migrationBuilder.RenameTable(
                name: "Tennets",
                newName: "Tenet");

            migrationBuilder.RenameColumn(
                name: "DateOfBirth",
                table: "Tenet",
                newName: "Dob");

            migrationBuilder.AlterColumn<string>(
                name: "MiddleName",
                table: "Tenet",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "PhotoId",
                table: "Tenet",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tenet",
                table: "Tenet",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Tenet",
                table: "Tenet");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "Tenet");

            migrationBuilder.RenameTable(
                name: "Tenet",
                newName: "Tennets");

            migrationBuilder.RenameColumn(
                name: "Dob",
                table: "Tennets",
                newName: "DateOfBirth");

            migrationBuilder.AlterColumn<string>(
                name: "MiddleName",
                table: "Tennets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tennets",
                table: "Tennets",
                column: "Id");
        }
    }
}
