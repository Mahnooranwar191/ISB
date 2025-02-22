using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISB.Migrations
{
    /// <inheritdoc />
    public partial class my : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_tasks_tbl_risks_risk_id",
                table: "tbl_tasks");

            migrationBuilder.DropIndex(
                name: "IX_tbl_tasks_risk_id",
                table: "tbl_tasks");

            migrationBuilder.DropColumn(
                name: "risk_id",
                table: "tbl_tasks");

            migrationBuilder.AddColumn<int>(
                name: "riskid",
                table: "tbl_tasks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_tasks_riskid",
                table: "tbl_tasks",
                column: "riskid");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_tasks_tbl_risks_riskid",
                table: "tbl_tasks",
                column: "riskid",
                principalTable: "tbl_risks",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_tasks_tbl_risks_riskid",
                table: "tbl_tasks");

            migrationBuilder.DropIndex(
                name: "IX_tbl_tasks_riskid",
                table: "tbl_tasks");

            migrationBuilder.DropColumn(
                name: "riskid",
                table: "tbl_tasks");

            migrationBuilder.AddColumn<int>(
                name: "risk_id",
                table: "tbl_tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_tasks_risk_id",
                table: "tbl_tasks",
                column: "risk_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_tasks_tbl_risks_risk_id",
                table: "tbl_tasks",
                column: "risk_id",
                principalTable: "tbl_risks",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
