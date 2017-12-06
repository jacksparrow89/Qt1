using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace QueryTracker.Data.Migrations
{
    public partial class queryTrackerSeven : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "QueryHistory",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QueryHistory_UserId",
                table: "QueryHistory",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_QueryHistory_AspNetUsers_UserId",
                table: "QueryHistory",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QueryHistory_AspNetUsers_UserId",
                table: "QueryHistory");

            migrationBuilder.DropIndex(
                name: "IX_QueryHistory_UserId",
                table: "QueryHistory");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "QueryHistory");
        }
    }
}
