using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TasksBoard.Backend.Domain;

namespace TasksBoard.Backend.Infrastructure.EntityTypeConfigurations
{
    public class UserBoardEntityTypeConfiguration : IEntityTypeConfiguration<UserBoard>
    {
        public void Configure(EntityTypeBuilder<UserBoard> builder)
        {
            builder.HasKey(t => new { t.UserId, t.BoardId });
            builder.HasOne(pt => pt.User)
                .WithMany(t => t.Boards)
                .HasForeignKey(pt => pt.UserId);

            builder.HasOne(pt => pt.Board)
                .WithMany(p => p.UserBoards)
                .HasForeignKey(pt => pt.BoardId);
        }
    }
}