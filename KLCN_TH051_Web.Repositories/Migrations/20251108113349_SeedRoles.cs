using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KLCN_TH051_Web.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                  table: "Roles", // Tên bảng trong DbContext của bạn
                  columns: new[]
                  {
                    "Id",
                    "Name",
                    "NormalizedName",
                    "ConcurrencyStamp",
                    "CreatedDate",
                    "IsDeleted"
                  },
                  values: new object[,]
                  {
                    { 1, "Student", "STUDENT", "seed-student", new DateTime(2025, 11, 8, 12, 0, 0), false },
                    { 2, "Teacher", "TEACHER", "seed-teacher", new DateTime(2025, 11, 8, 12, 0, 0), false },
                    { 3, "Admin",   "ADMIN",   "seed-admin",   new DateTime(2025, 11, 8, 12, 0, 0), false }
                  }
              );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                 table: "Roles",
                 keyColumn: "Id",
                 keyValues: new object[] { 1, 2, 3 }
             );
        }
    }
}
