using Microsoft.EntityFrameworkCore.Migrations;

namespace SCCB.Web.Migrations
{
    public partial class LessonType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Lessons",
                maxLength: 9,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Lessons");
        }
    }
}
