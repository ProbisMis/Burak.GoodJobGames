using Microsoft.EntityFrameworkCore;
using GoodJobGames.Data.EntityModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
            //builder.HasOne(c => c.Score);
        }
        //builder.HasOne(c => c.Status); //.WithMany(a => a.Appointments).HasForeignKey(c => c.StatusId);
        //builder.HasOne(c => c.Type);//.WithMany(a => a.Appointments).HasForeignKey(c => c.TypeId);
        //builder.HasOne(c => c.Slot);//.WithMany(a => a.Appointments).HasForeignKey(c => c.SlotId);
        //builder.HasOne(c => c.AppointmentReview).WithOne(a => a.Appointment);
    }
}
