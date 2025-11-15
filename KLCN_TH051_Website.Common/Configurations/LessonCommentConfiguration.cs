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
    public class LessonCommentConfiguration : IEntityTypeConfiguration<LessonComment>
    {
        public void Configure(EntityTypeBuilder<LessonComment> builder)
        {
            // Tên bảng
            builder.ToTable("LessonComments");

            // Khóa chính
            builder.HasKey(c => c.Id);

            // Nội dung comment
            builder.Property(c => c.Comment)
                   .IsRequired()
                   .HasMaxLength(1000);

            // FK Student
            builder.HasOne(c => c.Student)
                   .WithMany() // Nếu ApplicationUser không có navigation property tới Comment
                   .HasForeignKey(c => c.StudentId)
                   .OnDelete(DeleteBehavior.Restrict); // Không xóa comment khi xóa student

            // FK Lesson
            builder.HasOne(c => c.Lesson)
                   .WithMany(l => l.LessonComments)
                   .HasForeignKey(c => c.LessonId)
                   .OnDelete(DeleteBehavior.Cascade); // Xóa comment khi xóa lesson

            // FK ParentComment (self-reference)
            builder.HasOne(c => c.ParentComment)
                   .WithMany(c => c.Replies)
                   .HasForeignKey(c => c.ParentCommentId)
                   .OnDelete(DeleteBehavior.Restrict); // Tránh vòng lặp cascade
        }
    }
}
