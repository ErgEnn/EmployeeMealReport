using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class emp_id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealDays_Employees_EmployeeId",
                table: "MealDays");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "MealDays",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MealDays_Employees_EmployeeId",
                table: "MealDays",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealDays_Employees_EmployeeId",
                table: "MealDays");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "MealDays",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_MealDays_Employees_EmployeeId",
                table: "MealDays",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
