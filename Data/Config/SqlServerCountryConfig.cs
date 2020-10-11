using Microsoft.EntityFrameworkCore;
using Burak.GoodJobGames.Data.EntityModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Burak.GoodJobGames.Data.Config
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
