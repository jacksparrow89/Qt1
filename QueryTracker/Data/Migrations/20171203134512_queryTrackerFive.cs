using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace QueryTracker.Data.Migrations
{
    public partial class queryTrackerFive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Query_AspNetUsers_AssignedToId",
                table: "Query");

            migrationBuilder.DropIndex(
                name: "IX_Query_AssignedToId",
                table: "Query");

            migrationBuilder.DropColumn(
                name: "AssignedToId",
                table: "Query");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Query",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Query_UserId",
                table: "Query",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Query_AspNetUsers_UserId",
                table: "Query",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Query_AspNetUsers_UserId",
                table: "Query");

            migrationBuilder.DropIndex(
                name: "IX_Query_UserId",
                table: "Query");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Query");

            migrationBuilder.AddColumn<string>(
                name: "AssignedToId",
                table: "Query",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Query_AssignedToId",
                table: "Query",
                column: "AssignedToId");

            migrationBuilder.AddForeignKey(
                name: "FK_Query_AspNetUsers_AssignedToId",
                table: "Query",
                column: "AssignedToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
