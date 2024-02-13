using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsparagusN.Migrations
{
    /// <inheritdoc />
    public partial class ssssssssv : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "OrderItems",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<double>(
                name: "PricePerCarb",
                table: "BasketItems",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PricePerProtein",
                table: "BasketItems",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricePerCarb",
                table: "BasketItems");

            migrationBuilder.DropColumn(
                name: "PricePerProtein",
                table: "BasketItems");

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
