using Microsoft.EntityFrameworkCore;
using Burak.GoodJobGames.Data.EntityModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Burak.GoodJobGames.Data.Config
{
    public class SqlServerScoreConfig : IEntityTypeConfiguration<Score>
    {
        public void Configure(EntityTypeBuilder<Score> builder)
        {
            builder.ToTable(nameof(Score));
            builder.HasKey(model => model.Id);
            builder.HasOne(x => x.User).WithOne(c => c.Score).HasPrincipalKey<User>(x => x.GID).HasForeignKey<Score>(x => x.UserId);
        }
    }
}
