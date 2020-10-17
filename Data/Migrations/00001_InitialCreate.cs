using FluentMigrator;
using GoodJobGames.Data.EntityModels;
using System;

namespace GoodJobGames.Data.Migrations
{
    [Migration(1)]
    public partial class _00001_InitialCreate : Migration
    {
        public override void Up()
        {

            Create.Table(nameof(Country)) //TODO: Seed with sample country
              .WithColumn("Id").AsInt32().PrimaryKey().Identity()
              .WithColumn("CountryName").AsString()
              .WithColumn("CountryIsoCode").AsString();

            //Seed
            Insert.IntoTable(nameof(Country)).Row(new { CountryName = "Turkey", CountryIsoCode = "TR" });
            Insert.IntoTable(nameof(Country)).Row(new { CountryName = "United States", CountryIsoCode = "US" });
            Insert.IntoTable(nameof(Country)).Row(new { CountryName = "United Kingdom", CountryIsoCode = "UK" });
            Insert.IntoTable(nameof(Country)).Row(new { CountryName = "France", CountryIsoCode = "FR" });

            Create.Table(nameof(User)) //Seed with sample users and give guid on documentation
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("GID").AsGuid().Unique().NotNullable()
                .WithColumn("CountryId").AsInt32().ForeignKey(nameof(Country), "Id")
                .WithColumn("Username").AsString().Nullable()
                .WithColumn("Password").AsString().Nullable()
                .WithColumn("IsActive").AsBoolean().WithDefaultValue(true)
                .WithColumn("IsDeleted").AsBoolean().WithDefaultValue(false)
                .WithColumn("CreatedOnUtc").AsDateTime().WithDefaultValue(DateTime.Now)
                .WithColumn("UpdatedOnUtc").AsDateTime().WithDefaultValue(DateTime.Now)
                .WithColumn("Token").AsString().Nullable();

            //Seed
            //Insert.IntoTable(nameof(User)).Row(new { GID = Guid.NewGuid(), Username = "GJG-1" , Password = "123456", CountryId = 1});
            //Insert.IntoTable(nameof(User)).Row(new { GID = Guid.NewGuid(), Username = "GJG-2" , Password = "123456", CountryId = 1 });
            //Insert.IntoTable(nameof(User)).Row(new { GID = Guid.NewGuid(), Username = "GJG-3" , Password = "123456", CountryId = 1 });
            //Insert.IntoTable(nameof(User)).Row(new { GID = Guid.NewGuid(), Username = "GJG-4" , Password = "123456", CountryId = 2 });
            //Insert.IntoTable(nameof(User)).Row(new { GID = Guid.NewGuid(), Username = "GJG-5" , Password = "123456", CountryId = 2 });


            Create.Table(nameof(UserScore))
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("UserId").AsGuid().ForeignKey(nameof(User), "GID")
                .WithColumn("Score").AsInt32();

            ////Seed
            //Insert.IntoTable(nameof(Score)).Row(new { Id = 1, CountryId = 1, UserScore = 100 });
            //Insert.IntoTable(nameof(Score)).Row(new { UserId = 2, CountryId = 1, UserScore = 200 });
            //Insert.IntoTable(nameof(Score)).Row(new { UserId = 3, CountryId = 1, UserScore = 300 });



        }

        public override void Down()
        {
            Delete.Table(nameof(User));
            Delete.Table(nameof(Country));

            Delete.Table(nameof(UserScore));
        }
    }
}
