using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class fixpkonroles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_tb_r_employees_overtimes_tb_m_employees_employee_id",
                table: "tb_r_employees_overtimes");

            migrationBuilder.AlterColumn<string>(
                name: "employee_id",
                table: "tb_r_employees_overtimes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "tb_r_employees_overtimes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "tb_m_roles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_tb_r_employees_overtimes_tb_m_employees_employee_id",
                table: "tb_r_employees_overtimes",
                column: "employee_id",
                principalTable: "tb_m_employees",
                principalColumn: "nik",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_tb_r_employees_overtimes_tb_m_employees_employee_id",
                table: "tb_r_employees_overtimes");

            migrationBuilder.AlterColumn<string>(
                name: "employee_id",
                table: "tb_r_employees_overtimes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "tb_r_employees_overtimes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "tb_m_roles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "fk_tb_r_employees_overtimes_tb_m_employees_employee_id",
                table: "tb_r_employees_overtimes",
                column: "employee_id",
                principalTable: "tb_m_employees",
                principalColumn: "nik",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
