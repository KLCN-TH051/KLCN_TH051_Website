using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KLCN_TH051_Web.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class MakeCourseIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherAssignments_Courses_CourseId",
                table: "TeacherAssignments");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "TeacherAssignments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherAssignments_Courses_CourseId",
                table: "TeacherAssignments",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherAssignments_Courses_CourseId",
                table: "TeacherAssignments");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "TeacherAssignments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherAssignments_Courses_CourseId",
                table: "TeacherAssignments",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
