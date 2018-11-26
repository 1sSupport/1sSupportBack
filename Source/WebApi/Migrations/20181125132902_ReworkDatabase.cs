// --------------------------------------------------------------------------------------------------------------------
// <copyright file="20181125132902_ReworkDatabase.cs" company="">
//   
// </copyright>
// <summary>
//   The rework database.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Migrations
{
    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <summary>
    /// The rework database.
    /// </summary>
    public partial class ReworkDatabase : Migration
    {
        /// <summary>
        /// The down.
        /// </summary>
        /// <param name="migrationBuilder">
        /// The migration builder.
        /// </param>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("AskTitle", "dbo");

            migrationBuilder.DropTable("SupportAsk", "dbo");

            migrationBuilder.DropColumn("SupportEmail", schema: "dbo", table: "Provider");

            migrationBuilder.DropColumn("Preview", schema: "dbo", table: "Article");

            migrationBuilder.RenameColumn("FileName", schema: "dbo", table: "Article", newName: "Text");
        }

        /// <summary>
        /// The up.
        /// </summary>
        /// <param name="migrationBuilder">
        /// The migration builder.
        /// </param>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn("Text", schema: "dbo", table: "Article", newName: "FileName");

            migrationBuilder.AddColumn<string>(
                "SupportEmail",
                schema: "dbo",
                table: "Provider",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>("Preview", schema: "dbo", table: "Article", nullable: true);

            migrationBuilder.CreateTable(
                "SupportAsk",
                schema: "dbo",
                columns: table => new
                                      {
                                          Id = table.Column<int>(nullable: false).Annotation(
                                              "SqlServer:ValueGenerationStrategy",
                                              SqlServerValueGenerationStrategy.IdentityColumn),
                                          Text = table.Column<string>(nullable: false),
                                          ContactInfo = table.Column<string>(nullable: false),
                                          SessionId = table.Column<int>(nullable: false)
                                      },
                constraints: table =>
                    {
                        table.PrimaryKey("PK_SupportAsk", x => x.Id);
                        table.ForeignKey(
                            "FK_SupportAsk_Session_SessionId",
                            x => x.SessionId,
                            principalSchema: "dbo",
                            principalTable: "Session",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    });

            migrationBuilder.CreateTable(
                "AskTitle",
                schema: "dbo",
                columns: table => new
                                      {
                                          Id = table.Column<int>(nullable: false).Annotation(
                                              "SqlServer:ValueGenerationStrategy",
                                              SqlServerValueGenerationStrategy.IdentityColumn),
                                          Text = table.Column<string>(nullable: false),
                                          SupportAskId = table.Column<int>(nullable: false)
                                      },
                constraints: table =>
                    {
                        table.PrimaryKey("PK_AskTitle", x => x.Id);
                        table.ForeignKey(
                            "FK_AskTitle_SupportAsk_SupportAskId",
                            x => x.SupportAskId,
                            principalSchema: "dbo",
                            principalTable: "SupportAsk",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    });

            migrationBuilder.CreateIndex(
                "IX_AskTitle_SupportAskId",
                schema: "dbo",
                table: "AskTitle",
                column: "SupportAskId");

            migrationBuilder.CreateIndex(
                "IX_SupportAsk_SessionId",
                schema: "dbo",
                table: "SupportAsk",
                column: "SessionId");
        }
    }
}