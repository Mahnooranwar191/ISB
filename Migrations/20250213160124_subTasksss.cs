using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISB.Migrations
{
    /// <inheritdoc />
    public partial class subTasksss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "risk_id",
                table: "tbl_subtask",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_subtask_risk_id",
                table: "tbl_subtask",
                column: "risk_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_subtask_tbl_risks_risk_id",
                table: "tbl_subtask",
                column: "risk_id",
                principalTable: "tbl_risks",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_subtask_tbl_risks_risk_id",
                table: "tbl_subtask");

            migrationBuilder.DropIndex(
                name: "IX_tbl_subtask_risk_id",
                table: "tbl_subtask");

            migrationBuilder.DropColumn(
                name: "risk_id",
                table: "tbl_subtask");
        }
    }
}
