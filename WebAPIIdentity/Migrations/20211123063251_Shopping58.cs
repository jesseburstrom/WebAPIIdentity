using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPIIdentity.Migrations
{
    public partial class Shopping58 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryCategories_Categories_CategoryId",
                table: "CategoryCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryCategories_Categories_CategoryId1",
                table: "CategoryCategories");

            migrationBuilder.DropIndex(
                name: "IX_CategoryCategories_CategoryId1",
                table: "CategoryCategories");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "CategoryCategories",
                newName: "Category2CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryCategories_CategoryId",
                table: "CategoryCategories",
                newName: "IX_CategoryCategories_Category2CategoryId");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId1",
                table: "CategoryCategories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Category1CategoryId",
                table: "CategoryCategories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId2",
                table: "CategoryCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryCategories_Category1CategoryId",
                table: "CategoryCategories",
                column: "Category1CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryCategories_Categories_Category1CategoryId",
                table: "CategoryCategories",
                column: "Category1CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryCategories_Categories_Category2CategoryId",
                table: "CategoryCategories",
                column: "Category2CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryCategories_Categories_Category1CategoryId",
                table: "CategoryCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryCategories_Categories_Category2CategoryId",
                table: "CategoryCategories");

            migrationBuilder.DropIndex(
                name: "IX_CategoryCategories_Category1CategoryId",
                table: "CategoryCategories");

            migrationBuilder.DropColumn(
                name: "Category1CategoryId",
                table: "CategoryCategories");

            migrationBuilder.DropColumn(
                name: "CategoryId2",
                table: "CategoryCategories");

            migrationBuilder.RenameColumn(
                name: "Category2CategoryId",
                table: "CategoryCategories",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryCategories_Category2CategoryId",
                table: "CategoryCategories",
                newName: "IX_CategoryCategories_CategoryId");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId1",
                table: "CategoryCategories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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
    }
}
