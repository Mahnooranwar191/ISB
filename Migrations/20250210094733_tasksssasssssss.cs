using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISB.Migrations
{
    /// <inheritdoc />
    public partial class tasksssasssssss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_tasks_tbl_projects_Project_id",
                table: "tbl_tasks");

            migrationBuilder.RenameColumn(
                name: "Project_id",
                table: "tbl_tasks",
                newName: "project_");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_tasks_Project_id",
                table: "tbl_tasks",
                newName: "IX_tbl_tasks_project_");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_tasks_tbl_projects_project_",
                table: "tbl_tasks",
                column: "project_",
                principalTable: "tbl_projects",
                principalColumn: "Project_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_tasks_tbl_projects_project_",
                table: "tbl_tasks");

            migrationBuilder.RenameColumn(
                name: "project_",
                table: "tbl_tasks",
                newName: "Project_id");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_tasks_project_",
                table: "tbl_tasks",
                newName: "IX_tbl_tasks_Project_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_tasks_tbl_projects_Project_id",
                table: "tbl_tasks",
                column: "Project_id",
                principalTable: "tbl_projects",
                principalColumn: "Project_id");
        }
    }
}
