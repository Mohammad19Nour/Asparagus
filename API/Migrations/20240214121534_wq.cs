using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsparagusN.Migrations
{
    /// <inheritdoc />
    public partial class wq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionAR",
                table: "UserChangedCarbs");

            migrationBuilder.DropColumn(
                name: "DescriptionEN",
                table: "UserChangedCarbs");

            migrationBuilder.RenameColumn(
                name: "PictureUrl",
                table: "UserChangedCarbs",
                newName: "ExtraInfo");

            migrationBuilder.AddColumn<double>(
                name: "Carb",
                table: "UserChangedCarbs",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Fat",
                table: "UserChangedCarbs",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Fiber",
                table: "UserChangedCarbs",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "UserChangedCarbs",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Protein",
                table: "UserChangedCarbs",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "UserChangedCarbs",
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Carb",
                table: "UserChangedCarbs");

            migrationBuilder.DropColumn(
                name: "Fat",
                table: "UserChangedCarbs");

            migrationBuilder.DropColumn(
                name: "Fiber",
                table: "UserChangedCarbs");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "UserChangedCarbs");

            migrationBuilder.DropColumn(
                name: "Protein",
                table: "UserChangedCarbs");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "UserChangedCarbs");

            migrationBuilder.RenameColumn(
                name: "ExtraInfo",
                table: "UserChangedCarbs",
                newName: "PictureUrl");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAR",
                table: "UserChangedCarbs",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEN",
                table: "UserChangedCarbs",
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
        }
    }
}
