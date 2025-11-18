using KLCN_TH051_Website.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Configurations
{
    public class TeacherAssignmentConfiguration : IEntityTypeConfiguration<TeacherAssignment>
    {
        public void Configure(EntityTypeBuilder<TeacherAssignment> builder)
        {
            builder.ToTable("TeacherAssignments");

            builder.HasKey(t => t.Id);

            // Teacher (FK -> AspNetUsers)
            builder.HasOne(t => t.Teacher)
                .WithMany()                     // Không map ngược từ User
                .HasForeignKey(t => t.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);
            // Restrict để tránh xóa giáo viên làm mất dữ liệu phân công

            // Subject (FK -> Subjects)
            builder.HasOne(t => t.Subject)
                .WithMany()
                .HasForeignKey(t => t.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(t => t.Course)
                   .WithMany(c => c.TeacherAssignments)
                   .HasForeignKey(t => t.CourseId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Nếu muốn tránh phân công trùng
            //builder.HasIndex(t => new { t.TeacherId, t.SubjectId })
            //    .IsUnique();
        }
    }
}
