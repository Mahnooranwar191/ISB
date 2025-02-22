using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISB.Migrations
{
    /// <inheritdoc />
    public partial class emp_typesmaritialststua : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "marriage_annivary_date",
                table: "tbl_employee");

            migrationBuilder.DropColumn(
                name: "notice_period_end_date",
                table: "tbl_employee");

            migrationBuilder.DropColumn(
                name: "notice_period_start_date",
                table: "tbl_employee");

            migrationBuilder.DropColumn(
                name: "probination_date_period",
                table: "tbl_employee");

            migrationBuilder.CreateTable(
                name: "tbl_emptypes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    colour = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    statuas = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_emptypes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_maritalstatus",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    colour = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    statuas = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_maritalstatus", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_emptypes");

            migrationBuilder.DropTable(
                name: "tbl_maritalstatus");

            migrationBuilder.AddColumn<string>(
                name: "marriage_annivary_date",
                table: "tbl_employee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "notice_period_end_date",
                table: "tbl_employee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "notice_period_start_date",
                table: "tbl_employee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "probination_date_period",
                table: "tbl_employee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
