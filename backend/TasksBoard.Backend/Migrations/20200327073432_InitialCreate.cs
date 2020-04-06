using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TasksBoard.Backend.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "TasksBoardContext");

            migrationBuilder.CreateTable(
                name: "Boards",
                schema: "TasksBoardContext",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "TasksBoardContext",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Biography = table.Column<string>(nullable: true),
                    Hash = table.Column<byte[]>(nullable: true),
                    Salt = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Columns",
                schema: "TasksBoardContext",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Header = table.Column<string>(nullable: true),
                    Color = table.Column<string>(nullable: true),
                    OrderNum = table.Column<int>(nullable: false),
                    BoardId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Columns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Columns_Boards_BoardId",
                        column: x => x.BoardId,
                        principalSchema: "TasksBoardContext",
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserBoards",
                schema: "TasksBoardContext",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    BoardId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBoards", x => new { x.UserId, x.BoardId });
                    table.ForeignKey(
                        name: "FK_UserBoards_Boards_BoardId",
                        column: x => x.BoardId,
                        principalSchema: "TasksBoardContext",
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBoards_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "TasksBoardContext",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                schema: "TasksBoardContext",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Header = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    OrderNum = table.Column<int>(nullable: false),
                    OwnerId = table.Column<Guid>(nullable: true),
                    ColumnId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Columns_ColumnId",
                        column: x => x.ColumnId,
                        principalSchema: "TasksBoardContext",
                        principalTable: "Columns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tasks_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "TasksBoardContext",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                schema: "TasksBoardContext",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    AuthorId = table.Column<Guid>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: true),
                    TaskId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalSchema: "TasksBoardContext",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Comments_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "TasksBoardContext",
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalSchema: "TasksBoardContext",
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserTasks",
                schema: "TasksBoardContext",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    TaskId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTasks", x => new { x.UserId, x.TaskId });
                    table.ForeignKey(
                        name: "FK_UserTasks_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalSchema: "TasksBoardContext",
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTasks_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "TasksBoardContext",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Columns_BoardId",
                schema: "TasksBoardContext",
                table: "Columns",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AuthorId",
                schema: "TasksBoardContext",
                table: "Comments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ParentId",
                schema: "TasksBoardContext",
                table: "Comments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_TaskId",
                schema: "TasksBoardContext",
                table: "Comments",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ColumnId",
                schema: "TasksBoardContext",
                table: "Tasks",
                column: "ColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_OwnerId",
                schema: "TasksBoardContext",
                table: "Tasks",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBoards_BoardId",
                schema: "TasksBoardContext",
                table: "UserBoards",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTasks_TaskId",
                schema: "TasksBoardContext",
                table: "UserTasks",
                column: "TaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments",
                schema: "TasksBoardContext");

            migrationBuilder.DropTable(
                name: "UserBoards",
                schema: "TasksBoardContext");

            migrationBuilder.DropTable(
                name: "UserTasks",
                schema: "TasksBoardContext");

            migrationBuilder.DropTable(
                name: "Tasks",
                schema: "TasksBoardContext");

            migrationBuilder.DropTable(
                name: "Columns",
                schema: "TasksBoardContext");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "TasksBoardContext");

            migrationBuilder.DropTable(
                name: "Boards",
                schema: "TasksBoardContext");
        }
    }
}
