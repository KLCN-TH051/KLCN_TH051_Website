using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KLCN_TH051_Web.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseToTeacherAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Thêm cột CourseId, nhưng cho phép NULL
            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "TeacherAssignments",
                type: "int",
                nullable: true); // <- quan trọng: nullable

            // 2. Tạo index
            migrationBuilder.CreateIndex(
                name: "IX_TeacherAssignments_CourseId",
                table: "TeacherAssignments",
                column: "CourseId");

            // 3. Thêm foreign key, cho phép restrict delete
            migrationBuilder.AddForeignKey(
                name: "FK_TeacherAssignments_Courses_CourseId",
                table: "TeacherAssignments",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict); // <- hoặc Cascade nếu muốn tự xoá khi course bị xoá
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
        name: "FK_TeacherAssignments_Courses_CourseId",
        table: "TeacherAssignments");

            migrationBuilder.DropIndex(
                name: "IX_TeacherAssignments_CourseId",
                table: "TeacherAssignments");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "TeacherAssignments");
        }
    }
}
