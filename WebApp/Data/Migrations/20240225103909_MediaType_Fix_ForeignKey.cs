using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApp.Data.Migrations
{
    public partial class MediaType_Fix_ForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryItem_MediaType_CategoryId",
                table: "CategoryItem");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryItem_MediaTypeId",
                table: "CategoryItem",
                column: "MediaTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryItem_MediaType_MediaTypeId",
                table: "CategoryItem",
                column: "MediaTypeId",
                principalTable: "MediaType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryItem_MediaType_MediaTypeId",
                table: "CategoryItem");

            migrationBuilder.DropIndex(
                name: "IX_CategoryItem_MediaTypeId",
                table: "CategoryItem");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryItem_MediaType_CategoryId",
                table: "CategoryItem",
                column: "CategoryId",
                principalTable: "MediaType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
