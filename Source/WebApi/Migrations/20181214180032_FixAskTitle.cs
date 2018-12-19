// --------------------------------------------------------------------------------------------------------------------
// <copyright file="20181214180032_FixAskTitle.cs" company="">
//   
// </copyright>
// <summary>
//   The fix ask title.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Migrations
{
    #region

    using Microsoft.EntityFrameworkCore.Migrations;

    #endregion

    /// <summary>
    /// The fix ask title.
    /// </summary>
    public partial class FixAskTitle : Migration
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
                "FK_SessionQuery_SearchingQuery_SearchingQueryId",
                schema: "dbo",
                table: "SessionQuery");

            migrationBuilder.DropForeignKey("FK_SupportAsk_AskTitle_AskTitle_Id", schema: "dbo", table: "SupportAsk");

            migrationBuilder.DropIndex("IX_SupportAsk_AskTitle_Id", schema: "dbo", table: "SupportAsk");

            migrationBuilder.DropColumn("AskTitle_Id", schema: "dbo", table: "SupportAsk");

            migrationBuilder.AlterColumn<int>(
                "SearchingQueryId",
                schema: "dbo",
                table: "SessionQuery",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                "SupportAskId",
                schema: "dbo",
                table: "AskTitle",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                "IX_AskTitle_SupportAskId",
                schema: "dbo",
                table: "AskTitle",
                column: "SupportAskId");

            migrationBuilder.AddForeignKey(
                "FK_AskTitle_SupportAsk_SupportAskId",
                schema: "dbo",
                table: "AskTitle",
                column: "SupportAskId",
                principalSchema: "dbo",
                principalTable: "SupportAsk",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                "FK_SessionQuery_SearchingQuery_SearchingQueryId",
                schema: "dbo",
                table: "SessionQuery",
                column: "SearchingQueryId",
                principalSchema: "dbo",
                principalTable: "SearchingQuery",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <summary>
        /// The up.
        /// </summary>
        /// <param name="migrationBuilder">
        /// The migration builder.
        /// </param>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("FK_AskTitle_SupportAsk_SupportAskId", schema: "dbo", table: "AskTitle");

            migrationBuilder.DropForeignKey(
                "FK_SessionQuery_SearchingQuery_SearchingQueryId",
                schema: "dbo",
                table: "SessionQuery");

            migrationBuilder.DropIndex("IX_AskTitle_SupportAskId", schema: "dbo", table: "AskTitle");

            migrationBuilder.DropColumn("SupportAskId", schema: "dbo", table: "AskTitle");

            migrationBuilder.AddColumn<int>(
                "AskTitle_Id",
                schema: "dbo",
                table: "SupportAsk",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                "SearchingQueryId",
                schema: "dbo",
                table: "SessionQuery",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                "IX_SupportAsk_AskTitle_Id",
                schema: "dbo",
                table: "SupportAsk",
                column: "AskTitle_Id");

            migrationBuilder.AddForeignKey(
                "FK_SessionQuery_SearchingQuery_SearchingQueryId",
                schema: "dbo",
                table: "SessionQuery",
                column: "SearchingQueryId",
                principalSchema: "dbo",
                principalTable: "SearchingQuery",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_SupportAsk_AskTitle_AskTitle_Id",
                schema: "dbo",
                table: "SupportAsk",
                column: "AskTitle_Id",
                principalSchema: "dbo",
                principalTable: "AskTitle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}