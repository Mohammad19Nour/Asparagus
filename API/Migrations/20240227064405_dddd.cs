using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsparagusN.Migrations
{
    /// <inheritdoc />
    public partial class dddd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Questions_FAQId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_FAQId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "FAQId",
                table: "Questions");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ParentFAQId",
                table: "Questions",
                column: "ParentFAQId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Questions_ParentFAQId",
                table: "Questions",
                column: "ParentFAQId",
                principalTable: "Questions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Questions_ParentFAQId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_ParentFAQId",
                table: "Questions");

            migrationBuilder.AddColumn<int>(
                name: "FAQId",
                table: "Questions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_FAQId",
                table: "Questions",
                column: "FAQId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Questions_FAQId",
                table: "Questions",
                column: "FAQId",
                principalTable: "Questions",
                principalColumn: "Id");
        }
    }
}
