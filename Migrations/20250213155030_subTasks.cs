using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISB.Migrations
{
    /// <inheritdoc />
    public partial class subTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_subtask",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tittle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    discription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    files_upload = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    emp_id = table.Column<int>(type: "int", nullable: false),
                    task_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_subtask", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_subtask_tbl_employee_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tbl_employee",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_subtask_tbl_tasks_task_id",
                        column: x => x.task_id,
                        principalTable: "tbl_tasks",
                        principalColumn: "task_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_subtask_emp_id",
                table: "tbl_subtask",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_subtask_task_id",
                table: "tbl_subtask",
                column: "task_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_subtask");
        }
    }
}
