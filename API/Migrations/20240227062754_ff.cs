using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsparagusN.Migrations
{
    /// <inheritdoc />
    public partial class ff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Questions_ParentId",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "Questions",
                newName: "FAQId");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_ParentId",
                table: "Questions",
                newName: "IX_Questions_FAQId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Questions_FAQId",
                table: "Questions",
                column: "FAQId",
                principalTable: "Questions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Questions_FAQId",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "FAQId",
                table: "Questions",
                newName: "ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_FAQId",
                table: "Questions",
                newName: "IX_Questions_ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Questions_ParentId",
                table: "Questions",
                column: "ParentId",
                principalTable: "Questions",
                principalColumn: "Id");
        }
    }
}
