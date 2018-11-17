namespace WebApi.Migrations
{
    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.EntityFrameworkCore.Migrations;
    using System;

    public partial class Initial : Migration
    {
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "ArticleTag", schema: "dbo");

            migrationBuilder.DropTable(name: "OpenedArticle", schema: "dbo");

            migrationBuilder.DropTable(name: "Tag", schema: "dbo");

            migrationBuilder.DropTable(name: "Article", schema: "dbo");

            migrationBuilder.DropTable(name: "SearchingQuery", schema: "dbo");

            migrationBuilder.DropTable(name: "ArticleDependencies", schema: "dbo");

            migrationBuilder.DropTable(name: "Session", schema: "dbo");

            migrationBuilder.DropTable(name: "Configuration1C", schema: "dbo");

            migrationBuilder.DropTable(name: "User", schema: "dbo");

            migrationBuilder.DropTable(name: "Provider", schema: "dbo");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(name: "dbo");

            migrationBuilder.CreateTable(
                name: "Configuration1C",
                schema: "dbo",
                columns: table => new
                                      {
                                          Id = table.Column<int>(nullable: false).Annotation(
                                              "SqlServer:ValueGenerationStrategy",
                                              SqlServerValueGenerationStrategy.IdentityColumn),
                                          Name = table.Column<string>(nullable: false)
                                      },
                constraints: table => { table.PrimaryKey("PK_Configuration1C", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "Provider",
                schema: "dbo",
                columns: table => new
                                      {
                                          Id = table.Column<int>(nullable: false).Annotation(
                                              "SqlServer:ValueGenerationStrategy",
                                              SqlServerValueGenerationStrategy.IdentityColumn),
                                          Name = table.Column<string>(nullable: false),
                                          LogoUrl = table.Column<string>(maxLength: 300, nullable: true),
                                          ContractEndTime = table.Column<DateTime>(nullable: false)
                                      },
                constraints: table => { table.PrimaryKey("PK_Provider", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "Tag",
                schema: "dbo",
                columns: table => new
                                      {
                                          Id = table.Column<int>(nullable: false).Annotation(
                                              "SqlServer:ValueGenerationStrategy",
                                              SqlServerValueGenerationStrategy.IdentityColumn),
                                          Value = table.Column<string>(maxLength: 120, nullable: false)
                                      },
                constraints: table => { table.PrimaryKey("PK_Tag", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "ArticleDependencies",
                schema: "dbo",
                columns: table => new
                                      {
                                          Id = table.Column<int>(nullable: false).Annotation(
                                              "SqlServer:ValueGenerationStrategy",
                                              SqlServerValueGenerationStrategy.IdentityColumn),
                                          Configuration1CId = table.Column<int>(nullable: true)
                                      },
                constraints: table =>
                    {
                        table.PrimaryKey("PK_ArticleDependencies", x => x.Id);
                        table.ForeignKey(
                            name: "FK_ArticleDependencies_Configuration1C_Configuration1CId",
                            column: x => x.Configuration1CId,
                            principalSchema: "dbo",
                            principalTable: "Configuration1C",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Restrict);
                    });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "dbo",
                columns: table => new
                                      {
                                          Id = table.Column<int>(nullable: false).Annotation(
                                              "SqlServer:ValueGenerationStrategy",
                                              SqlServerValueGenerationStrategy.IdentityColumn),
                                          Login = table.Column<string>(nullable: false),
                                          Email = table.Column<string>(nullable: false),
                                          INN = table.Column<string>(maxLength: 12, nullable: false),
                                          ProviderId = table.Column<int>(nullable: true)
                                      },
                constraints: table =>
                    {
                        table.PrimaryKey("PK_User", x => x.Id);
                        table.ForeignKey(
                            name: "FK_User_Provider_ProviderId",
                            column: x => x.ProviderId,
                            principalSchema: "dbo",
                            principalTable: "Provider",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Restrict);
                    });

            migrationBuilder.CreateTable(
                name: "Article",
                schema: "dbo",
                columns: table => new
                                      {
                                          Id = table.Column<int>(nullable: false).Annotation(
                                              "SqlServer:ValueGenerationStrategy",
                                              SqlServerValueGenerationStrategy.IdentityColumn),
                                          Title = table.Column<string>(nullable: false),
                                          Text = table.Column<string>(nullable: false),
                                          EditDate = table.Column<DateTime>(nullable: true),
                                          Weight = table.Column<double>(nullable: true),
                                          IsDeleted = table.Column<bool>(nullable: true),
                                          ArticleDependenciesId = table.Column<int>(nullable: true)
                                      },
                constraints: table =>
                    {
                        table.PrimaryKey("PK_Article", x => x.Id);
                        table.ForeignKey(
                            name: "FK_Article_ArticleDependencies_ArticleDependenciesId",
                            column: x => x.ArticleDependenciesId,
                            principalSchema: "dbo",
                            principalTable: "ArticleDependencies",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Restrict);
                    });

            migrationBuilder.CreateTable(
                name: "Session",
                schema: "dbo",
                columns: table => new
                                      {
                                          Id = table.Column<int>(nullable: false).Annotation(
                                              "SqlServer:ValueGenerationStrategy",
                                              SqlServerValueGenerationStrategy.IdentityColumn),
                                          OpenTime = table.Column<DateTime>(nullable: false),
                                          CloseTime = table.Column<DateTime>(nullable: true),
                                          UserId = table.Column<int>(nullable: true)
                                      },
                constraints: table =>
                    {
                        table.PrimaryKey("PK_Session", x => x.Id);
                        table.ForeignKey(
                            name: "FK_Session_User_UserId",
                            column: x => x.UserId,
                            principalSchema: "dbo",
                            principalTable: "User",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Restrict);
                    });

            migrationBuilder.CreateTable(
                name: "ArticleTag",
                schema: "dbo",
                columns: table => new
                                      {
                                          ID = table.Column<int>(nullable: false).Annotation(
                                              "SqlServer:ValueGenerationStrategy",
                                              SqlServerValueGenerationStrategy.IdentityColumn),
                                          Weight = table.Column<double>(nullable: false),
                                          ArticleId = table.Column<int>(nullable: true),
                                          TagId = table.Column<int>(nullable: true)
                                      },
                constraints: table =>
                    {
                        table.PrimaryKey("PK_ArticleTag", x => x.ID);
                        table.ForeignKey(
                            name: "FK_ArticleTag_Article_ArticleId",
                            column: x => x.ArticleId,
                            principalSchema: "dbo",
                            principalTable: "Article",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Restrict);
                        table.ForeignKey(
                            name: "FK_ArticleTag_Tag_TagId",
                            column: x => x.TagId,
                            principalSchema: "dbo",
                            principalTable: "Tag",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Restrict);
                    });

            migrationBuilder.CreateTable(
                name: "SearchingQuery",
                schema: "dbo",
                columns: table => new
                                      {
                                          Id = table.Column<int>(nullable: false).Annotation(
                                              "SqlServer:ValueGenerationStrategy",
                                              SqlServerValueGenerationStrategy.IdentityColumn),
                                          Text = table.Column<string>(nullable: false),
                                          Time = table.Column<string>(nullable: false),
                                          SessionId = table.Column<int>(nullable: true)
                                      },
                constraints: table =>
                    {
                        table.PrimaryKey("PK_SearchingQuery", x => x.Id);
                        table.ForeignKey(
                            name: "FK_SearchingQuery_Session_SessionId",
                            column: x => x.SessionId,
                            principalSchema: "dbo",
                            principalTable: "Session",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Restrict);
                    });

            migrationBuilder.CreateTable(
                name: "OpenedArticle",
                schema: "dbo",
                columns: table => new
                                      {
                                          Id = table.Column<int>(nullable: false).Annotation(
                                              "SqlServer:ValueGenerationStrategy",
                                              SqlServerValueGenerationStrategy.IdentityColumn),
                                          OpenedNumber = table.Column<int>(nullable: false),
                                          ListNumber = table.Column<int>(nullable: true),
                                          IsLastOpened = table.Column<bool>(nullable: true),
                                          Time = table.Column<DateTime>(nullable: false),
                                          Mark = table.Column<int>(nullable: true),
                                          ArticleId = table.Column<int>(nullable: true),
                                          SearchingQueryId = table.Column<int>(nullable: true)
                                      },
                constraints: table =>
                    {
                        table.PrimaryKey("PK_OpenedArticle", x => x.Id);
                        table.ForeignKey(
                            name: "FK_OpenedArticle_Article_ArticleId",
                            column: x => x.ArticleId,
                            principalSchema: "dbo",
                            principalTable: "Article",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Restrict);
                        table.ForeignKey(
                            name: "FK_OpenedArticle_SearchingQuery_SearchingQueryId",
                            column: x => x.SearchingQueryId,
                            principalSchema: "dbo",
                            principalTable: "SearchingQuery",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Restrict);
                    });

            migrationBuilder.CreateIndex(
                name: "IX_Article_ArticleDependenciesId",
                schema: "dbo",
                table: "Article",
                column: "ArticleDependenciesId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleDependencies_Configuration1CId",
                schema: "dbo",
                table: "ArticleDependencies",
                column: "Configuration1CId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTag_ArticleId",
                schema: "dbo",
                table: "ArticleTag",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTag_TagId",
                schema: "dbo",
                table: "ArticleTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenedArticle_ArticleId",
                schema: "dbo",
                table: "OpenedArticle",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenedArticle_SearchingQueryId",
                schema: "dbo",
                table: "OpenedArticle",
                column: "SearchingQueryId");

            migrationBuilder.CreateIndex(
                name: "IX_SearchingQuery_SessionId",
                schema: "dbo",
                table: "SearchingQuery",
                column: "SessionId");

            migrationBuilder.CreateIndex(name: "IX_Session_UserId", schema: "dbo", table: "Session", column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_ProviderId",
                schema: "dbo",
                table: "User",
                column: "ProviderId");
        }
    }
}