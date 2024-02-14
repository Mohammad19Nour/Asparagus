using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsparagusN.Migrations
{
    /// <inheritdoc />
    public partial class sc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdminSelectedCarbs_Meals_CarbId",
                table: "AdminSelectedCarbs");

            migrationBuilder.DropColumn(
                name: "DescriptionAR",
                table: "UserSelectedSnacks");

            migrationBuilder.DropColumn(
                name: "DescriptionEN",
                table: "UserSelectedSnacks");

            migrationBuilder.RenameColumn(
                name: "PictureUrl",
                table: "UserSelectedSnacks",
                newName: "ExtraInfo");

            migrationBuilder.RenameColumn(
                name: "Fibers",
                table: "UserSelectedSnacks",
                newName: "Weight");

            migrationBuilder.RenameColumn(
                name: "Fats",
                table: "UserSelectedSnacks",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "Carbs",
                table: "UserSelectedSnacks",
                newName: "Fiber");

            migrationBuilder.RenameColumn(
                name: "Calories",
                table: "UserSelectedSnacks",
                newName: "Fat");

            migrationBuilder.AddColumn<double>(
                name: "Carb",
                table: "UserSelectedSnacks",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "OrderItems",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddForeignKey(
                name: "FK_AdminSelectedCarbs_Ingredients_CarbId",
                table: "AdminSelectedCarbs",
                column: "CarbId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdminSelectedCarbs_Ingredients_CarbId",
                table: "AdminSelectedCarbs");

            migrationBuilder.DropColumn(
                name: "Carb",
                table: "UserSelectedSnacks");

            migrationBuilder.RenameColumn(
                name: "Weight",
                table: "UserSelectedSnacks",
                newName: "Fibers");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "UserSelectedSnacks",
                newName: "Fats");

            migrationBuilder.RenameColumn(
                name: "Fiber",
                table: "UserSelectedSnacks",
                newName: "Carbs");

            migrationBuilder.RenameColumn(
                name: "Fat",
                table: "UserSelectedSnacks",
                newName: "Calories");

            migrationBuilder.RenameColumn(
                name: "ExtraInfo",
                table: "UserSelectedSnacks",
                newName: "PictureUrl");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAR",
                table: "UserSelectedSnacks",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEN",
                table: "UserSelectedSnacks",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "OrderItems",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddForeignKey(
                name: "FK_AdminSelectedCarbs_Meals_CarbId",
                table: "AdminSelectedCarbs",
                column: "CarbId",
                principalTable: "Meals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
