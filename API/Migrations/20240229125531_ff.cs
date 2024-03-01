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
                name: "FK_Orders_Address_ShipToAddressId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Drivers_DriverId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DriverId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ShipToAddressId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShipToAddressId",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "DriverId",
                table: "UserPlanDays",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "UserPlanDays",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "OrderItems",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPlanDays_DriverId",
                table: "UserPlanDays",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPlanDays_Drivers_DriverId",
                table: "UserPlanDays",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPlanDays_Drivers_DriverId",
                table: "UserPlanDays");

            migrationBuilder.DropIndex(
                name: "IX_UserPlanDays_DriverId",
                table: "UserPlanDays");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "UserPlanDays");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "UserPlanDays");

            migrationBuilder.AddColumn<int>(
                name: "DriverId",
                table: "Orders",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "Orders",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShipToAddressId",
                table: "Orders",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "OrderItems",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DriverId",
                table: "Orders",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShipToAddressId",
                table: "Orders",
                column: "ShipToAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Address_ShipToAddressId",
                table: "Orders",
                column: "ShipToAddressId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Drivers_DriverId",
                table: "Orders",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
