using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCCB.Web.Migrations
{
    public partial class GroupsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AcademicGroupId",
                table: "Students",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Groups",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Students_AcademicGroupId",
                table: "Students",
                column: "AcademicGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_OwnerId",
                table: "Groups",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Users_OwnerId",
                table: "Groups",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Groups_AcademicGroupId",
                table: "Students",
                column: "AcademicGroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Users_OwnerId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Groups_AcademicGroupId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_AcademicGroupId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Groups_OwnerId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "AcademicGroupId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Groups");
        }
    }
}
