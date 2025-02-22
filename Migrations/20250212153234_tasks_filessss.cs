using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISB.Migrations
{
    /// <inheritdoc />
    public partial class tasks_filessss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "tbl_taks_file");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "task_id",
                table: "tbl_taks_file",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "user_id",
                table: "tbl_taks_file",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
    }
}
