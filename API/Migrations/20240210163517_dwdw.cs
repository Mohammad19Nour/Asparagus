using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsparagusN.Migrations
{
    /// <inheritdoc />
    public partial class dwdw : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderedMeal_Name",
                table: "OrderItems",
                newName: "OrderedMeal_PictureUrl");

            migrationBuilder.RenameColumn(
                name: "OrderedMeal_Details",
                table: "OrderItems",
                newName: "OrderedMeal_NameEN");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "OrderItems",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "OrderedMeal_AddedCarb",
                table: "OrderItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderedMeal_AddedProtein",
                table: "OrderItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OrderedMeal_DescriptionAR",
                table: "OrderItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OrderedMeal_DescriptionEN",
                table: "OrderItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "OrderedMeal_MealId",
                table: "OrderItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OrderedMeal_NameAR",
                table: "OrderItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderedMeal_AddedCarb",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "OrderedMeal_AddedProtein",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "OrderedMeal_DescriptionAR",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "OrderedMeal_DescriptionEN",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "OrderedMeal_MealId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "OrderedMeal_NameAR",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "OrderedMeal_PictureUrl",
                table: "OrderItems",
                newName: "OrderedMeal_Name");

            migrationBuilder.RenameColumn(
                name: "OrderedMeal_NameEN",
                table: "OrderItems",
                newName: "OrderedMeal_Details");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "OrderItems",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);
        }
    }
}
