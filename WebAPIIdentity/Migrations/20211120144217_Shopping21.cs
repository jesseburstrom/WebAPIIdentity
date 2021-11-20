using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPIIdentity.Migrations
{
    public partial class Shopping21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoryCategories",
                columns: table => new
                {
                    CategoryCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    CategoryId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryCategories", x => x.CategoryCategoryId);
                    table.ForeignKey(
                        name: "FK_CategoryCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CategoryCategories_Categories_CategoryId1",
                        column: x => x.CategoryId1,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryCategories_CategoryId",
                table: "CategoryCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryCategories_CategoryId1",
                table: "CategoryCategories",
                column: "CategoryId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryCategories");
        }
    }
}
