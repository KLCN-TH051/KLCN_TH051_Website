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
     public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            // Tên bảng
            builder.ToTable("Questions");

            // Khóa chính
            builder.HasKey(q => q.Id);

            builder.Property(q => q.QuestionText)
                   .IsRequired()
                   .HasMaxLength(1000);

            builder.Property(q => q.Points)
                   .IsRequired()
                   .HasColumnType("decimal(5,2)")  // hỗ trợ điểm ví dụ 0.25
                   .HasDefaultValue(1.0m);

            builder.Property(q => q.OrderNumber)
                   .IsRequired()
                   .HasDefaultValue(0);

            // Quan hệ với Quiz
            builder.HasOne(q => q.Quiz)
                   .WithMany(qz => qz.Questions)
                   .HasForeignKey(q => q.QuizId)
                   .OnDelete(DeleteBehavior.Cascade);

            //// Quan hệ với Answer
            //builder.HasMany(q => q.Answers)
            //       .WithOne(a => a.Question)
            //       .HasForeignKey(a => a.QuestionId)
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
