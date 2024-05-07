using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NemEcommerce.Migrations
{
    /// <inheritdoc />
    public partial class nemchay12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverPicture",
                table: "AppProductCategories");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "AppProductCategories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoverPicture",
                table: "AppProductCategories",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "AppProductCategories",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
