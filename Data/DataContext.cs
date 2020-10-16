﻿using Microsoft.EntityFrameworkCore;
using GoodJobGames.Data.EntityModels;

namespace GoodJobGames.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserScore> Scores { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        //public virtual DbSet<AppointmentReview> AppointmentReviews { get; set; }
        //public virtual DbSet<Status> Statuses { get; set; }
        //public virtual DbSet<Slot> Slots { get; set; }
        //public virtual DbSet<Type> Types { get; set; }
    }
}
