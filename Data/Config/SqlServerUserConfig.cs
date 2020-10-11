using Microsoft.EntityFrameworkCore;
using Burak.GoodJobGames.Data.EntityModels;

namespace Burak.GoodJobGames.Data.Config
{
    public class SqlServerUserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User));
            builder.HasKey(model => model.Id);
            //builder.HasOne(c => c.Status); //.WithMany(a => a.Appointments).HasForeignKey(c => c.StatusId);
            //builder.HasOne(c => c.Type);//.WithMany(a => a.Appointments).HasForeignKey(c => c.TypeId);
            //builder.HasOne(c => c.Slot);//.WithMany(a => a.Appointments).HasForeignKey(c => c.SlotId);
            //builder.HasOne(c => c.AppointmentReview).WithOne(a => a.Appointment);
        }
    }
}
