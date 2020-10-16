using Microsoft.EntityFrameworkCore;
using GoodJobGames.Data.EntityModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodJobGames.Data.Config
{
    public class SqlServerScoreConfig : IEntityTypeConfiguration<UserScore>
    {
        public void Configure(EntityTypeBuilder<UserScore> builder)
        {
            builder.ToTable(nameof(UserScore));
            builder.HasKey(model => model.Id);
            builder.HasOne(x => x.User).WithOne(c => c.Score).HasPrincipalKey<User>(x => x.GID).HasForeignKey<UserScore>(x => x.UserId);
        }
    }
}
