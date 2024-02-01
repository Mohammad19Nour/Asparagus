using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsparagusN.Migrations
{
    /// <inheritdoc />
    public partial class ttt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdminSelectedMeals_AdminPlan_AdminPlanId",
                table: "AdminSelectedMeals");

            migrationBuilder.DropForeignKey(
                name: "FK_DrinkItem_AdminPlan_AdminPlanId",
                table: "DrinkItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdminPlan",
                table: "AdminPlan");

            migrationBuilder.RenameTable(
                name: "AdminPlan",
                newName: "AdminPlans");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Orders",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "OrderItems",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AvailableDate",
                table: "AdminPlans",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdminPlans",
                table: "AdminPlans",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AdminSelectedMeals_AdminPlans_AdminPlanId",
                table: "AdminSelectedMeals",
                column: "AdminPlanId",
                principalTable: "AdminPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DrinkItem_AdminPlans_AdminPlanId",
                table: "DrinkItem",
                column: "AdminPlanId",
                principalTable: "AdminPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdminSelectedMeals_AdminPlans_AdminPlanId",
                table: "AdminSelectedMeals");

            migrationBuilder.DropForeignKey(
                name: "FK_DrinkItem_AdminPlans_AdminPlanId",
                table: "DrinkItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdminPlans",
                table: "AdminPlans");

            migrationBuilder.RenameTable(
                name: "AdminPlans",
                newName: "AdminPlan");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Orders",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "OrderItems",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AvailableDate",
                table: "AdminPlan",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdminPlan",
                table: "AdminPlan",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AdminSelectedMeals_AdminPlan_AdminPlanId",
                table: "AdminSelectedMeals",
                column: "AdminPlanId",
                principalTable: "AdminPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DrinkItem_AdminPlan_AdminPlanId",
                table: "DrinkItem",
                column: "AdminPlanId",
                principalTable: "AdminPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
