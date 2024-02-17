using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsparagusN.Migrations
{
    /// <inheritdoc />
    public partial class wsw : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserPlanDayId1",
                table: "UserSelectedExtraOptions",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryCity",
                table: "UserPlans",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "UserPlans",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "OrderItems",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.CreateTable(
                name: "UserPlanAllergy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ArabicName = table.Column<string>(type: "TEXT", nullable: false),
                    EnglishName = table.Column<string>(type: "TEXT", nullable: false),
                    PictureUrl = table.Column<string>(type: "TEXT", nullable: false),
                    UserPlanId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPlanAllergy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPlanAllergy_UserPlans_UserPlanId",
                        column: x => x.UserPlanId,
                        principalTable: "UserPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSelectedExtraOptions_UserPlanDayId1",
                table: "UserSelectedExtraOptions",
                column: "UserPlanDayId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlanAllergy_UserPlanId",
                table: "UserPlanAllergy",
                column: "UserPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSelectedExtraOptions_UserPlanDays_UserPlanDayId1",
                table: "UserSelectedExtraOptions",
                column: "UserPlanDayId1",
                principalTable: "UserPlanDays",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSelectedExtraOptions_UserPlanDays_UserPlanDayId1",
                table: "UserSelectedExtraOptions");

            migrationBuilder.DropTable(
                name: "UserPlanAllergy");

            migrationBuilder.DropIndex(
                name: "IX_UserSelectedExtraOptions_UserPlanDayId1",
                table: "UserSelectedExtraOptions");

            migrationBuilder.DropColumn(
                name: "UserPlanDayId1",
                table: "UserSelectedExtraOptions");

            migrationBuilder.DropColumn(
                name: "DeliveryCity",
                table: "UserPlans");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "UserPlans");

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
