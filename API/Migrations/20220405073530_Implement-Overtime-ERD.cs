using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class ImplementOvertimeERD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_m_jobs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    min_salary = table.Column<int>(type: "int", nullable: false),
                    max_salary = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tb_m_jobs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_overtimes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    submit_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    paid = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tb_m_overtimes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tb_m_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_employees",
                columns: table => new
                {
                    nik = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    gender = table.Column<int>(type: "int", nullable: false),
                    salary = table.Column<int>(type: "int", nullable: false),
                    job_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tb_m_employees", x => x.nik);
                    table.ForeignKey(
                        name: "fk_tb_m_employees_tb_m_jobs_job_id",
                        column: x => x.job_id,
                        principalTable: "tb_m_jobs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_accounts",
                columns: table => new
                {
                    nik = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    otp = table.Column<int>(type: "int", nullable: false),
                    is_used = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tb_m_accounts", x => x.nik);
                    table.ForeignKey(
                        name: "fk_tb_m_accounts_tb_m_employees_employee_nik",
                        column: x => x.nik,
                        principalTable: "tb_m_employees",
                        principalColumn: "nik",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_r_employees_overtimes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    @break = table.Column<DateTime>(name: "break", type: "datetime2", nullable: false),
                    start_overtime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_overtime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    employee_id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    overtime_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tb_r_employees_overtimes", x => x.id);
                    table.ForeignKey(
                        name: "fk_tb_r_employees_overtimes_tb_m_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "tb_m_employees",
                        principalColumn: "nik",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_tb_r_employees_overtimes_tb_m_overtimes_overtime_id",
                        column: x => x.overtime_id,
                        principalTable: "tb_m_overtimes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_accounts_roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    account_nik = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    role_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tb_m_accounts_roles", x => x.id);
                    table.ForeignKey(
                        name: "fk_tb_m_accounts_roles_tb_m_accounts_account_nik",
                        column: x => x.account_nik,
                        principalTable: "tb_m_accounts",
                        principalColumn: "nik",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_tb_m_accounts_roles_tb_m_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "tb_m_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_tb_m_accounts_roles_account_nik",
                table: "tb_m_accounts_roles",
                column: "account_nik");

            migrationBuilder.CreateIndex(
                name: "ix_tb_m_accounts_roles_role_id",
                table: "tb_m_accounts_roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_tb_m_employees_job_id",
                table: "tb_m_employees",
                column: "job_id");

            migrationBuilder.CreateIndex(
                name: "ix_tb_r_employees_overtimes_employee_id",
                table: "tb_r_employees_overtimes",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "ix_tb_r_employees_overtimes_overtime_id",
                table: "tb_r_employees_overtimes",
                column: "overtime_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_m_accounts_roles");

            migrationBuilder.DropTable(
                name: "tb_r_employees_overtimes");

            migrationBuilder.DropTable(
                name: "tb_m_accounts");

            migrationBuilder.DropTable(
                name: "tb_m_roles");

            migrationBuilder.DropTable(
                name: "tb_m_overtimes");

            migrationBuilder.DropTable(
                name: "tb_m_employees");

            migrationBuilder.DropTable(
                name: "tb_m_jobs");
        }
    }
}
