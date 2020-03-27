using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace SCCB.Web.Migrations
{
    public partial class ChangePasswordToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChangePasswordToken",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationChangePasswordTokenDate",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangePasswordToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ExpirationChangePasswordTokenDate",
                table: "Users");
        }
    }
}
