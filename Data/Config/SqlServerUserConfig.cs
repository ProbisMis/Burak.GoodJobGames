using Microsoft.EntityFrameworkCore;
using GoodJobGames.Data.EntityModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodJobGames.Data.Config
{
    public class SqlServerUserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User));
            builder.HasKey(model => model.Id);
            builder.HasAlternateKey(c => c.GID);
            builder.HasOne(x => x.Country).WithOne(c => c.User).HasForeignKey<User>(x => x.CountryId);
        }
    }
}
