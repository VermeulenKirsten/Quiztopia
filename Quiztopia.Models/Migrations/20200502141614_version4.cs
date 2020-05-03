using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Quiztopia.Models.Migrations
{
    public partial class version4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnswerString",
                table: "Answers");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Questions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PossibleAnswer",
                table: "Answers",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "PossibleAnswer",
                table: "Answers");

            migrationBuilder.AddColumn<string>(
                name: "AnswerString",
                table: "Answers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
