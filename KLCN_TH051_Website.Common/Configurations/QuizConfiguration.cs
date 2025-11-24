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
    public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
            // Tên bảng
            builder.ToTable("Quizzes");

            // Khóa chính
            builder.HasKey(q => q.Id);

            builder.Property(q => q.Title)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(q => q.Description)
                   .HasMaxLength(1000)
                   .IsRequired(false);

            builder.Property(q => q.Type)
                   .IsRequired(); // Enum QuizType, EF Core tự map thành int hoặc string tùy cấu hình

            builder.Property(q => q.PassingScore)
                   .IsRequired();

            builder.Property(q => q.TimeLimitMinutes)
                   .IsRequired();

            builder.Property(q => q.MaxAttempts)
                   .IsRequired(false); // có thể null nếu không giới hạn số lần làm

            // Quan hệ với Lesson
            builder.HasOne(q => q.Lesson)
                   .WithOne()
                   .HasForeignKey<Quiz>(q => q.LessonId)
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
