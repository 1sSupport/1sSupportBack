// --------------------------------------------------------------------------------------------------------------------
// <copyright file="20181111214340_FixQuery.cs" company="">
//   
// </copyright>
// <summary>
//   The fix query.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebApi.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    /// <summary>
    /// The fix query.
    /// </summary>
    public partial class FixQuery : Migration
    {
        /// <summary>
        /// The down.
        /// </summary>
        /// <param name="migrationBuilder">
        /// The migration builder.
        /// </param>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                "Time",
                schema: "dbo",
                table: "SearchingQuery",
                nullable: false,
                oldClrType: typeof(DateTime));
        }

        /// <summary>
        /// The up.
        /// </summary>
        /// <param name="migrationBuilder">
        /// The migration builder.
        /// </param>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                "Time",
                schema: "dbo",
                table: "SearchingQuery",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}