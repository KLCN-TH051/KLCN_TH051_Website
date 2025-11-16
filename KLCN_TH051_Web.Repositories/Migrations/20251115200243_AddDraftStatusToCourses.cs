using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KLCN_TH051_Web.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddDraftStatusToCourses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Courses",
                nullable: false,
                defaultValue: 3, // Draft
                oldClrType: typeof(int),
                oldNullable: false);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
               name: "Status",
               table: "Courses",
               nullable: false,
               oldClrType: typeof(int),
               oldDefaultValue: 3);
        }
    }
}
