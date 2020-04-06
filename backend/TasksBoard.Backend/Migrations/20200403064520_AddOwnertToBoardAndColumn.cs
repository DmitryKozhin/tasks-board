using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TasksBoard.Backend.Migrations
{
    public partial class AddOwnertToBoardAndColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                schema: "TasksBoardContext",
                table: "Columns",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                schema: "TasksBoardContext",
                table: "Boards",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Columns_OwnerId",
                schema: "TasksBoardContext",
                table: "Columns",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Boards_OwnerId",
                schema: "TasksBoardContext",
                table: "Boards",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Boards_Users_OwnerId",
                schema: "TasksBoardContext",
                table: "Boards",
                column: "OwnerId",
                principalSchema: "TasksBoardContext",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Columns_Users_OwnerId",
                schema: "TasksBoardContext",
                table: "Columns",
                column: "OwnerId",
                principalSchema: "TasksBoardContext",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boards_Users_OwnerId",
                schema: "TasksBoardContext",
                table: "Boards");

            migrationBuilder.DropForeignKey(
                name: "FK_Columns_Users_OwnerId",
                schema: "TasksBoardContext",
                table: "Columns");

            migrationBuilder.DropIndex(
                name: "IX_Columns_OwnerId",
                schema: "TasksBoardContext",
                table: "Columns");

            migrationBuilder.DropIndex(
                name: "IX_Boards_OwnerId",
                schema: "TasksBoardContext",
                table: "Boards");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                schema: "TasksBoardContext",
                table: "Columns");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                schema: "TasksBoardContext",
                table: "Boards");
        }
    }
}
