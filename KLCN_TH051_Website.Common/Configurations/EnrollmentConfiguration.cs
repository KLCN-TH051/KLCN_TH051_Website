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
    public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            // Tên bảng
            builder.ToTable("Enrollments");

            // Khóa chính
            builder.HasKey(e => e.Id);

            // Liên kết với Student (ApplicationUser)
            builder.HasOne(e => e.Student)
                   .WithMany() // Nếu ApplicationUser không có navigation property tới Enrollments
                   .HasForeignKey(e => e.StudentId)
                   .OnDelete(DeleteBehavior.Restrict); 

            // Liên kết với Course
            builder.HasOne(e => e.Course)
                   .WithMany(c => c.Enrollments)
                   .HasForeignKey(e => e.CourseId)
                   .OnDelete(DeleteBehavior.Restrict); // Không xóa khóa học khi xóa Enrollment

            // Trạng thái enrollment là enum
            builder.Property(e => e.Status)
                   .HasConversion<int>()
                   .IsRequired();

            // Các thuộc tính khác
            builder.Property(e => e.EnrolledDate)
                   .IsRequired();

            builder.Property(e => e.ProgressPercentage)
                   .HasDefaultValue(0);

            // Quan hệ với LessonProgress
            //builder.HasMany(e => e.LessonProgresses)
            //       .WithOne(lp => lp.Enrollment)
            //       .HasForeignKey(lp => lp.EnrollmentId)
            //       .OnDelete(DeleteBehavior.Cascade);

            // ----------------------------
            // Cấu hình các cột từ BaseEntity
            // ----------------------------

            builder.Property(c => c.CreatedDate)
                   .HasDefaultValueSql("GETDATE()"); // default ngày tạo

            builder.Property(c => c.LastUpdatedDate)
                   .IsRequired(false); // optional

            builder.Property(c => c.DeletedTime)
                   .IsRequired(false); // optional

            builder.Property(c => c.IsDeleted)
                   .HasDefaultValue(false); // mặc định false

            builder.Property(c => c.CreatedBy)
                   .HasMaxLength(50)
                   .IsRequired(false);

            builder.Property(c => c.LastUpdatedBy)
                   .HasMaxLength(50)
                   .IsRequired(false);

            builder.Property(c => c.DeletedBy)
                   .HasMaxLength(50)
                   .IsRequired(false);
        }
    }
}
