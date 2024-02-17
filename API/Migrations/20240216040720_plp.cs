using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsparagusN.Migrations
{
    /// <inheritdoc />
    public partial class plp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSelectedExtraOptions_UserPlanDays_UserPlanDayId1",
                table: "UserSelectedExtraOptions");

            migrationBuilder.DropIndex(
                name: "IX_UserSelectedExtraOptions_UserPlanDayId1",
                table: "UserSelectedExtraOptions");

            migrationBuilder.DropColumn(
                name: "UserPlanDayId1",
                table: "UserSelectedExtraOptions");

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
            migrationBuilder.AddColumn<int>(
                name: "UserPlanDayId1",
                table: "UserSelectedExtraOptions",
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
                name: "IX_UserSelectedExtraOptions_UserPlanDayId1",
                table: "UserSelectedExtraOptions",
                column: "UserPlanDayId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSelectedExtraOptions_UserPlanDays_UserPlanDayId1",
                table: "UserSelectedExtraOptions",
                column: "UserPlanDayId1",
                principalTable: "UserPlanDays",
                principalColumn: "Id");
        }
    }
}
