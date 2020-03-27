using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TasksBoard.Backend.Domain;

namespace TasksBoard.Backend.Infrastructure.Context
{
    public class UserTaskEntityTypeConfiguration : IEntityTypeConfiguration<UserTask>
    {
        public void Configure(EntityTypeBuilder<UserTask> builder)
        {
            builder.HasKey(t => new { t.UserId, t.TaskId });
            builder.HasOne(pt => pt.User)
                .WithMany(t => t.Tasks)
                .HasForeignKey(pt => pt.UserId);

            builder.HasOne(pt => pt.Task)
                .WithMany(p => p.AssignedUsers)
                .HasForeignKey(pt => pt.TaskId);
        }
    }
}