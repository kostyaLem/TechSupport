using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechSupport.DataAccess.Migrations
{
    public partial class CreateCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestCategories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestCategories_Title",
                table: "RequestCategories",
                column: "Title");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestCategories");
        }
    }
}
