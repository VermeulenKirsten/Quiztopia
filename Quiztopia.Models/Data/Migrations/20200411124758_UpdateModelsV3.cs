using Microsoft.EntityFrameworkCore.Migrations;

namespace Quiztopia.Models.Data.Migrations
{
    public partial class UpdateModelsV3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Topics",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Quizzes",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_DifficultyId",
                table: "Quizzes",
                column: "DifficultyId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_TopicId",
                table: "Quizzes",
                column: "TopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Difficulties_DifficultyId",
                table: "Quizzes",
                column: "DifficultyId",
                principalTable: "Difficulties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Topics_TopicId",
                table: "Quizzes",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Difficulties_DifficultyId",
                table: "Quizzes");

            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Topics_TopicId",
                table: "Quizzes");

            migrationBuilder.DropIndex(
                name: "IX_Quizzes_DifficultyId",
                table: "Quizzes");

            migrationBuilder.DropIndex(
                name: "IX_Quizzes_TopicId",
                table: "Quizzes");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Topics",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 25);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Quizzes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);
        }
    }
}
