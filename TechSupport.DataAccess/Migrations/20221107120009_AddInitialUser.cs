using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechSupport.DataAccess.Migrations
{
    public partial class AddInitialUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            var dateTime = DateTime.Now;
            migrationBuilder.Sql(
                "INSERT INTO dbo.Users\r\n" +
                "VALUES ('admin', 'admin', '1998-06-15', 'admin@gmail.com', '89211452356', " +
                "'admin', 'x61Ey612Kl2gpFL56FT9weDnpSo4AV8j8+qx2AuTHdRyY036xxzTTrw10Wq3+4qQyB+XURPWx1ONxp3Y3pB37A==', " +
                $"'Admin', '{dateTime}', '{dateTime}', '{dateTime}')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
