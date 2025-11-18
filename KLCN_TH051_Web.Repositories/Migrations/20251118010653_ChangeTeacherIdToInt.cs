using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KLCN_TH051_Web.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTeacherIdToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1️⃣ Xóa FK & Index cũ (nếu có)
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherAssignments_Users_TeacherId1",
                table: "TeacherAssignments");

            migrationBuilder.DropIndex(
                name: "IX_TeacherAssignments_TeacherId1",
                table: "TeacherAssignments");

            migrationBuilder.DropColumn(
                name: "TeacherId1",
                table: "TeacherAssignments");

            // 2️⃣ Convert dữ liệu cũ từ string sang int
            migrationBuilder.Sql(@"
                UPDATE TeacherAssignments
                SET TeacherId = CAST(TeacherId AS int)
            ");

            // 3️⃣ Alter column sang int
            migrationBuilder.AlterColumn<int>(
                name: "TeacherId",
                table: "TeacherAssignments",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            // 4️⃣ Tạo Index mới
            migrationBuilder.CreateIndex(
                name: "IX_TeacherAssignments_TeacherId",
                table: "TeacherAssignments",
                column: "TeacherId");

            // 5️⃣ Tạo FK mới (NO ACTION để tránh multiple cascade paths)
            migrationBuilder.AddForeignKey(
                name: "FK_TeacherAssignments_Users_TeacherId",
                table: "TeacherAssignments",
                column: "TeacherId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict); // ✅ Tránh lỗi cascade
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 1️⃣ Xóa FK & Index
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherAssignments_Users_TeacherId",
                table: "TeacherAssignments");

            migrationBuilder.DropIndex(
                name: "IX_TeacherAssignments_TeacherId",
                table: "TeacherAssignments");

            // 2️⃣ Alter column về string
            migrationBuilder.AlterColumn<string>(
                name: "TeacherId",
                table: "TeacherAssignments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            // 3️⃣ Tạo lại cột phụ & Index cũ (nếu cần rollback)
            migrationBuilder.AddColumn<int>(
                name: "TeacherId1",
                table: "TeacherAssignments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherAssignments_TeacherId1",
                table: "TeacherAssignments",
                column: "TeacherId1");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherAssignments_Users_TeacherId1",
                table: "TeacherAssignments",
                column: "TeacherId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
