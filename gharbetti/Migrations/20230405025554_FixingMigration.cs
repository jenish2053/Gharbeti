using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gharbetti.Migrations
{
    /// <inheritdoc />
    public partial class FixingMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tennets");

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                table: "Transactions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                table: "TenantMessages",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_PaymentModeId",
                table: "Transactions",
                column: "PaymentModeId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TenantId",
                table: "Transactions",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionDetails_ExpenseId",
                table: "TransactionDetails",
                column: "ExpenseId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionDetails_TransactionId",
                table: "TransactionDetails",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantMessages_MessageId",
                table: "TenantMessages",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantMessages_TenantId",
                table: "TenantMessages",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_FloorId",
                table: "Rooms",
                column: "FloorId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomDetails_RoomId",
                table: "RoomDetails",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomDetails_RoomTypeId",
                table: "RoomDetails",
                column: "RoomTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_HouseRooms_HouseId",
                table: "HouseRooms",
                column: "HouseId");

            migrationBuilder.CreateIndex(
                name: "IX_HouseRooms_RoomId",
                table: "HouseRooms",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_HouseRooms_Houses_HouseId",
                table: "HouseRooms",
                column: "HouseId",
                principalTable: "Houses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HouseRooms_Rooms_RoomId",
                table: "HouseRooms",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomDetails_RoomTypes_RoomTypeId",
                table: "RoomDetails",
                column: "RoomTypeId",
                principalTable: "RoomTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomDetails_Rooms_RoomId",
                table: "RoomDetails",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Floors_FloorId",
                table: "Rooms",
                column: "FloorId",
                principalTable: "Floors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TenantMessages_AspNetUsers_TenantId",
                table: "TenantMessages",
                column: "TenantId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TenantMessages_Message_MessageId",
                table: "TenantMessages",
                column: "MessageId",
                principalTable: "Message",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionDetails_ExpenseTypes_ExpenseId",
                table: "TransactionDetails",
                column: "ExpenseId",
                principalTable: "ExpenseTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionDetails_Transactions_TransactionId",
                table: "TransactionDetails",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_AspNetUsers_TenantId",
                table: "Transactions",
                column: "TenantId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_PaymentModes_PaymentModeId",
                table: "Transactions",
                column: "PaymentModeId",
                principalTable: "PaymentModes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HouseRooms_Houses_HouseId",
                table: "HouseRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_HouseRooms_Rooms_RoomId",
                table: "HouseRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomDetails_RoomTypes_RoomTypeId",
                table: "RoomDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomDetails_Rooms_RoomId",
                table: "RoomDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Floors_FloorId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_TenantMessages_AspNetUsers_TenantId",
                table: "TenantMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_TenantMessages_Message_MessageId",
                table: "TenantMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionDetails_ExpenseTypes_ExpenseId",
                table: "TransactionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionDetails_Transactions_TransactionId",
                table: "TransactionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_AspNetUsers_TenantId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_PaymentModes_PaymentModeId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_PaymentModeId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_TenantId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_TransactionDetails_ExpenseId",
                table: "TransactionDetails");

            migrationBuilder.DropIndex(
                name: "IX_TransactionDetails_TransactionId",
                table: "TransactionDetails");

            migrationBuilder.DropIndex(
                name: "IX_TenantMessages_MessageId",
                table: "TenantMessages");

            migrationBuilder.DropIndex(
                name: "IX_TenantMessages_TenantId",
                table: "TenantMessages");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_FloorId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_RoomDetails_RoomId",
                table: "RoomDetails");

            migrationBuilder.DropIndex(
                name: "IX_RoomDetails_RoomTypeId",
                table: "RoomDetails");

            migrationBuilder.DropIndex(
                name: "IX_HouseRooms_HouseId",
                table: "HouseRooms");

            migrationBuilder.DropIndex(
                name: "IX_HouseRooms_RoomId",
                table: "HouseRooms");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "TenantMessages",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "Tennets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tennets", x => x.Id);
                });
        }
    }
}
