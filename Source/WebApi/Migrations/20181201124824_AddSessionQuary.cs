using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class AddSessionQuary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpenedArticle_SearchingQuery_SearchingQueryId",
                schema: "dbo",
                table: "OpenedArticle");

            migrationBuilder.DropForeignKey(
                name: "FK_SearchingQuery_Session_SessionId",
                schema: "dbo",
                table: "SearchingQuery");

            migrationBuilder.DropIndex(
                name: "IX_SearchingQuery_SessionId",
                schema: "dbo",
                table: "SearchingQuery");

            migrationBuilder.DropColumn(
                name: "SessionId",
                schema: "dbo",
                table: "SearchingQuery");

            migrationBuilder.DropColumn(
                name: "Time",
                schema: "dbo",
                table: "SearchingQuery");

            migrationBuilder.AddColumn<int>(
                name: "Amount",
                schema: "dbo",
                table: "SearchingQuery",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SessionQuery",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Time = table.Column<DateTime>(nullable: false),
                    SessionId = table.Column<int>(nullable: true),
                    SearchingQueryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionQuery", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionQuery_SearchingQuery_SearchingQueryId",
                        column: x => x.SearchingQueryId,
                        principalSchema: "dbo",
                        principalTable: "SearchingQuery",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionQuery_Session_SessionId",
                        column: x => x.SessionId,
                        principalSchema: "dbo",
                        principalTable: "Session",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SessionQuery_SearchingQueryId",
                schema: "dbo",
                table: "SessionQuery",
                column: "SearchingQueryId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionQuery_SessionId",
                schema: "dbo",
                table: "SessionQuery",
                column: "SessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_OpenedArticle_SessionQuery_SearchingQueryId",
                schema: "dbo",
                table: "OpenedArticle",
                column: "SearchingQueryId",
                principalSchema: "dbo",
                principalTable: "SessionQuery",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpenedArticle_SessionQuery_SearchingQueryId",
                schema: "dbo",
                table: "OpenedArticle");

            migrationBuilder.DropTable(
                name: "SessionQuery",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "Amount",
                schema: "dbo",
                table: "SearchingQuery");

            migrationBuilder.AddColumn<int>(
                name: "SessionId",
                schema: "dbo",
                table: "SearchingQuery",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Time",
                schema: "dbo",
                table: "SearchingQuery",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_SearchingQuery_SessionId",
                schema: "dbo",
                table: "SearchingQuery",
                column: "SessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_OpenedArticle_SearchingQuery_SearchingQueryId",
                schema: "dbo",
                table: "OpenedArticle",
                column: "SearchingQueryId",
                principalSchema: "dbo",
                principalTable: "SearchingQuery",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SearchingQuery_Session_SessionId",
                schema: "dbo",
                table: "SearchingQuery",
                column: "SessionId",
                principalSchema: "dbo",
                principalTable: "Session",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
