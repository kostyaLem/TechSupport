using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechSupport.DataAccess.Migrations
{
    public partial class ExchangeDateColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedOn",
                table: "Requests");

            migrationBuilder.RenameColumn(
                name: "PausedOn",
                table: "Requests",
                newName: "StatusUpdatedOn");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StatusUpdatedOn",
                table: "Requests",
                newName: "PausedOn");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedOn",
                table: "Requests",
                type: "datetime2",
                nullable: true);
        }
    }
}
