using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TasksBoard.Backend.Migrations
{
    public partial class AddIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Columns_Boards_BoardId",
                schema: "TasksBoardContext",
                table: "Columns");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Columns_ColumnId",
                schema: "TasksBoardContext",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_OwnerId",
                schema: "TasksBoardContext",
                table: "Tasks");

            migrationBuilder.AlterColumn<Guid>(
                name: "OwnerId",
                schema: "TasksBoardContext",
                table: "Tasks",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ColumnId",
                schema: "TasksBoardContext",
                table: "Tasks",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "BoardId",
                schema: "TasksBoardContext",
                table: "Columns",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Columns_Boards_BoardId",
                schema: "TasksBoardContext",
                table: "Columns",
                column: "BoardId",
                principalSchema: "TasksBoardContext",
                principalTable: "Boards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Columns_ColumnId",
                schema: "TasksBoardContext",
                table: "Tasks",
                column: "ColumnId",
                principalSchema: "TasksBoardContext",
                principalTable: "Columns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_OwnerId",
                schema: "TasksBoardContext",
                table: "Tasks",
                column: "OwnerId",
                principalSchema: "TasksBoardContext",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Columns_Boards_BoardId",
                schema: "TasksBoardContext",
                table: "Columns");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Columns_ColumnId",
                schema: "TasksBoardContext",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_OwnerId",
                schema: "TasksBoardContext",
                table: "Tasks");

            migrationBuilder.AlterColumn<Guid>(
                name: "OwnerId",
                schema: "TasksBoardContext",
                table: "Tasks",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "ColumnId",
                schema: "TasksBoardContext",
                table: "Tasks",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "BoardId",
                schema: "TasksBoardContext",
                table: "Columns",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_Columns_Boards_BoardId",
                schema: "TasksBoardContext",
                table: "Columns",
                column: "BoardId",
                principalSchema: "TasksBoardContext",
                principalTable: "Boards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Columns_ColumnId",
                schema: "TasksBoardContext",
                table: "Tasks",
                column: "ColumnId",
                principalSchema: "TasksBoardContext",
                principalTable: "Columns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_OwnerId",
                schema: "TasksBoardContext",
                table: "Tasks",
                column: "OwnerId",
                principalSchema: "TasksBoardContext",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
