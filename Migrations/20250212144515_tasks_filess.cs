using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISB.Migrations
{
    /// <inheritdoc />
    public partial class tasks_filess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_taks_file_tbl_tasks_task_ids",
                table: "tbl_taks_file");

            migrationBuilder.DropIndex(
                name: "IX_tbl_taks_file_task_ids",
                table: "tbl_taks_file");

            migrationBuilder.AddColumn<int>(
                name: "task_id",
                table: "tbl_taks_file",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_taks_file_task_id",
                table: "tbl_taks_file",
                column: "task_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_taks_file_tbl_tasks_task_id",
                table: "tbl_taks_file",
                column: "task_id",
                principalTable: "tbl_tasks",
                principalColumn: "task_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_taks_file_tbl_tasks_task_id",
                table: "tbl_taks_file");

            migrationBuilder.DropIndex(
                name: "IX_tbl_taks_file_task_id",
                table: "tbl_taks_file");

            migrationBuilder.DropColumn(
                name: "task_id",
                table: "tbl_taks_file");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_taks_file_task_ids",
                table: "tbl_taks_file",
                column: "task_ids");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_taks_file_tbl_tasks_task_ids",
                table: "tbl_taks_file",
                column: "task_ids",
                principalTable: "tbl_tasks",
                principalColumn: "task_id");
        }
    }
}
