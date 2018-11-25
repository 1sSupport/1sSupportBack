using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class ReworkDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Text",
                schema: "dbo",
                table: "Article",
                newName: "FileName");

            migrationBuilder.AddColumn<string>(
                name: "SupportEmail",
                schema: "dbo",
                table: "Provider",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Preview",
                schema: "dbo",
                table: "Article",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SupportAsk",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Text = table.Column<string>(nullable: false),
                    ContactInfo = table.Column<string>(nullable: false),
                    SessionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportAsk", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupportAsk_Session_SessionId",
                        column: x => x.SessionId,
                        principalSchema: "dbo",
                        principalTable: "Session",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AskTitle",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Text = table.Column<string>(nullable: false),
                    SupportAskId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AskTitle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AskTitle_SupportAsk_SupportAskId",
                        column: x => x.SupportAskId,
                        principalSchema: "dbo",
                        principalTable: "SupportAsk",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AskTitle_SupportAskId",
                schema: "dbo",
                table: "AskTitle",
                column: "SupportAskId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportAsk_SessionId",
                schema: "dbo",
                table: "SupportAsk",
                column: "SessionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AskTitle",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SupportAsk",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "SupportEmail",
                schema: "dbo",
                table: "Provider");

            migrationBuilder.DropColumn(
                name: "Preview",
                schema: "dbo",
                table: "Article");

            migrationBuilder.RenameColumn(
                name: "FileName",
                schema: "dbo",
                table: "Article",
                newName: "Text");
        }
    }
}
