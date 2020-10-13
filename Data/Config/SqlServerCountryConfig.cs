using Microsoft.EntityFrameworkCore;
using GoodJobGames.Data.EntityModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodJobGames.Data.Config
{
    public class SqlServerCountryConfig : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.ToTable(nameof(Country));
            builder.HasKey(model => model.Id);
        }
    }
}
