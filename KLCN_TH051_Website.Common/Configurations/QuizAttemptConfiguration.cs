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
    public class QuizAttemptConfiguration : IEntityTypeConfiguration<QuizAttempt>
    {
        public void Configure(EntityTypeBuilder<QuizAttempt> builder)
        {
            // Tên bảng
            builder.ToTable("QuizAttempts");

            // Khóa chính
            builder.HasKey(qa => qa.Id);

            // Properties
            builder.Property(qa => qa.Id)
                   .IsRequired()
                   .HasMaxLength(50); // nếu dùng string GUID

            builder.Property(qa => qa.Score)
                   .IsRequired();

            builder.Property(qa => qa.Passed)
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.Property(qa => qa.AttemptNumber)
                   .IsRequired();

            builder.Property(qa => qa.AttemptDate)
                   .IsRequired()
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(qa => qa.Answers)
                   .IsRequired()
                   .HasColumnType("nvarchar(max)"); // lưu JSON

            // Quan hệ với Student (ApplicationUser)
            builder.HasOne(qa => qa.Student)
                   .WithMany() // cần ICollection<QuizAttempt> trong ApplicationUser
                   .HasForeignKey(qa => qa.StudentId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ với Quiz
            builder.HasOne(qa => qa.Quiz)
                   .WithMany(q => q.Attempts)
                   .HasForeignKey(qa => qa.QuizId)
                   .OnDelete(DeleteBehavior.Cascade);

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
