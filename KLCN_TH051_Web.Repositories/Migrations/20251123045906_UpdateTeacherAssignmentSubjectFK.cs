using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KLCN_TH051_Web.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTeacherAssignmentSubjectFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Xoá FK cũ
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherAssignments_Subjects_SubjectId",
                table: "TeacherAssignments");

            // Thêm FK mới với Restrict
            migrationBuilder.AddForeignKey(
                name: "FK_TeacherAssignments_Subjects_SubjectId",
                table: "TeacherAssignments",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Quay lại Cascade nếu rollback
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherAssignments_Subjects_SubjectId",
                table: "TeacherAssignments");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherAssignments_Subjects_SubjectId",
                table: "TeacherAssignments",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
