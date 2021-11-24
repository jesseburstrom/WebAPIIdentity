using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPIIdentity.Migrations
{
    public partial class Shopping57 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "CategoryCategories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId1",
                table: "CategoryCategories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryCategories_CategoryId",
                table: "CategoryCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryCategories_CategoryId1",
                table: "CategoryCategories",
                column: "CategoryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryCategories_Categories_CategoryId",
                table: "CategoryCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryCategories_Categories_CategoryId1",
                table: "CategoryCategories",
                column: "CategoryId1",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryCategories_Categories_CategoryId",
                table: "CategoryCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryCategories_Categories_CategoryId1",
                table: "CategoryCategories");

            migrationBuilder.DropIndex(
                name: "IX_CategoryCategories_CategoryId",
                table: "CategoryCategories");

            migrationBuilder.DropIndex(
                name: "IX_CategoryCategories_CategoryId1",
                table: "CategoryCategories");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "CategoryCategories");

            migrationBuilder.DropColumn(
                name: "CategoryId1",
                table: "CategoryCategories");
        }
    }
}
