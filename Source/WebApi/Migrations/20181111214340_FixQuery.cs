namespace WebApi.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;
    using System;

    public partial class FixQuery : Migration
    {
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Time",
                schema: "dbo",
                table: "SearchingQuery",
                nullable: false,
                oldClrType: typeof(DateTime));
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Time",
                schema: "dbo",
                table: "SearchingQuery",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}