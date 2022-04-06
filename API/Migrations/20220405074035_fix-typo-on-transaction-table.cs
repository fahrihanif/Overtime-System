using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class fixtypoontransactiontable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "break",
                table: "tb_r_employees_overtimes",
                newName: "break_overtime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "break_overtime",
                table: "tb_r_employees_overtimes",
                newName: "break");
        }
    }
}
