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

            Create.Table(nameof(Country)) 
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
                .WithColumn("CountryId").AsInt32().Nullable().ForeignKey(nameof(Country), "Id")
                .WithColumn("Username").AsString().Nullable()
                .WithColumn("Password").AsString().Nullable()
                .WithColumn("IsActive").AsBoolean().WithDefaultValue(true)
                .WithColumn("IsDeleted").AsBoolean().WithDefaultValue(false)
                .WithColumn("CreatedOnUtc").AsDateTime2().WithDefaultValue(DateTime.Now)
                .WithColumn("UpdatedOnUtc").AsDateTime2().WithDefaultValue(DateTime.Now);


            Create.Table(nameof(UserScore))
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("UserId").AsGuid().ForeignKey(nameof(User), "GID")
                .WithColumn("Score").AsInt32();
        }

        public override void Down()
        {
            Delete.Table(nameof(User));
            Delete.Table(nameof(Country));

            Delete.Table(nameof(UserScore));
        }
    }
}
