using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KLCN_TH051_Web.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLessonDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Lessons");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Lessons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
