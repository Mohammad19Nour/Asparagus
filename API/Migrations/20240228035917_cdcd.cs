using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsparagusN.Migrations
{
    /// <inheritdoc />
    public partial class cdcd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Questions_ParentFAQId1",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_ParentFAQId1",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "ParentFAQId1",
                table: "Questions");

            migrationBuilder.AlterColumn<int>(
                name: "ParentFAQId",
                table: "Questions",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "OrderItems",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ParentFAQId",
                table: "Questions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentFAQId1",
                table: "Questions",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "OrderItems",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ParentFAQId1",
                table: "Questions",
                column: "ParentFAQId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Questions_ParentFAQId1",
                table: "Questions",
                column: "ParentFAQId1",
                principalTable: "Questions",
                principalColumn: "Id");
        }
    }
}
