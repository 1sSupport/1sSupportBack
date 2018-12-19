// --------------------------------------------------------------------------------------------------------------------
// <copyright file="20181201124824_AddSessionQuary.cs" company="">
//   
// </copyright>
// <summary>
//   The add session quary.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Migrations
{
    #region

    using System;

    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.EntityFrameworkCore.Migrations;

    #endregion

    /// <summary>
    /// The add session quary.
    /// </summary>
    public partial class AddSessionQuary : Migration
    {
        /// <summary>
        /// The down.
        /// </summary>
        /// <param name="migrationBuilder">
        /// The migration builder.
        /// </param>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_OpenedArticle_SessionQuery_SearchingQueryId",
                schema: "dbo",
                table: "OpenedArticle");

            migrationBuilder.DropTable("SessionQuery", "dbo");

            migrationBuilder.DropColumn("Amount", schema: "dbo", table: "SearchingQuery");

            migrationBuilder.AddColumn<int>("SessionId", schema: "dbo", table: "SearchingQuery", nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                "Time",
                schema: "dbo",
                table: "SearchingQuery",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                "IX_SearchingQuery_SessionId",
                schema: "dbo",
                table: "SearchingQuery",
                column: "SessionId");

            migrationBuilder.AddForeignKey(
                "FK_OpenedArticle_SearchingQuery_SearchingQueryId",
                schema: "dbo",
                table: "OpenedArticle",
                column: "SearchingQueryId",
                principalSchema: "dbo",
                principalTable: "SearchingQuery",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_SearchingQuery_Session_SessionId",
                schema: "dbo",
                table: "SearchingQuery",
                column: "SessionId",
                principalSchema: "dbo",
                principalTable: "Session",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <summary>
        /// The up.
        /// </summary>
        /// <param name="migrationBuilder">
        /// The migration builder.
        /// </param>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_OpenedArticle_SearchingQuery_SearchingQueryId",
                schema: "dbo",
                table: "OpenedArticle");

            migrationBuilder.DropForeignKey(
                "FK_SearchingQuery_Session_SessionId",
                schema: "dbo",
                table: "SearchingQuery");

            migrationBuilder.DropIndex("IX_SearchingQuery_SessionId", schema: "dbo", table: "SearchingQuery");

            migrationBuilder.DropColumn("SessionId", schema: "dbo", table: "SearchingQuery");

            migrationBuilder.DropColumn("Time", schema: "dbo", table: "SearchingQuery");

            migrationBuilder.AddColumn<int>(
                "Amount",
                schema: "dbo",
                table: "SearchingQuery",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                "SessionQuery",
                schema: "dbo",
                columns: table => new
                                      {
                                          Id = table.Column<int>(nullable: false).Annotation(
                                              "SqlServer:ValueGenerationStrategy",
                                              SqlServerValueGenerationStrategy.IdentityColumn),
                                          Time = table.Column<DateTime>(nullable: false),
                                          SessionId = table.Column<int>(nullable: true),
                                          SearchingQueryId = table.Column<int>(nullable: false)
                                      },
                constraints: table =>
                    {
                        table.PrimaryKey("PK_SessionQuery", x => x.Id);
                        table.ForeignKey(
                            "FK_SessionQuery_SearchingQuery_SearchingQueryId",
                            x => x.SearchingQueryId,
                            principalSchema: "dbo",
                            principalTable: "SearchingQuery",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                        table.ForeignKey(
                            "FK_SessionQuery_Session_SessionId",
                            x => x.SessionId,
                            principalSchema: "dbo",
                            principalTable: "Session",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Restrict);
                    });

            migrationBuilder.CreateIndex(
                "IX_SessionQuery_SearchingQueryId",
                schema: "dbo",
                table: "SessionQuery",
                column: "SearchingQueryId");

            migrationBuilder.CreateIndex(
                "IX_SessionQuery_SessionId",
                schema: "dbo",
                table: "SessionQuery",
                column: "SessionId");

            migrationBuilder.AddForeignKey(
                "FK_OpenedArticle_SessionQuery_SearchingQueryId",
                schema: "dbo",
                table: "OpenedArticle",
                column: "SearchingQueryId",
                principalSchema: "dbo",
                principalTable: "SessionQuery",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}