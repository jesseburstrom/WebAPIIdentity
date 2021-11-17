using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPIIdentity.Migrations
{
    public partial class Highscores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Topscores",
                table: "Topscores");

            migrationBuilder.RenameTable(
                name: "Topscores",
                newName: "Highscores");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Highscores",
                table: "Highscores",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Highscores",
                table: "Highscores");

            migrationBuilder.RenameTable(
                name: "Highscores",
                newName: "Topscores");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Topscores",
                table: "Topscores",
                column: "Id");
        }
    }
}
