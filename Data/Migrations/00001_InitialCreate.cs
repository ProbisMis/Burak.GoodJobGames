﻿using FluentMigrator;
using Burak.GoodJobGames.Data.EntityModels;
using System;

namespace Burak.GoodJobGames.Data.Migrations
{
    [Migration(1)]
    public partial class _00001_InitialCreate : Migration
    {
        public override void Up()
        {
            Create.Table(nameof(User)) //Seed with sample users and give guid on documentation
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("GID").AsGuid().Indexed().NotNullable()
                .WithColumn("Username").AsString().Nullable()
                .WithColumn("Password").AsString().Nullable()
                .WithColumn("IsActive").AsBoolean().WithDefaultValue(true)
                .WithColumn("IsDeleted").AsBoolean().WithDefaultValue(false)
                .WithColumn("CreatedOnUtc").AsDateTime().WithDefaultValue(DateTime.Now)
                .WithColumn("UpdatedOnUtc").AsDateTime().WithDefaultValue(DateTime.Now)
                .WithColumn("Token").AsString().Nullable();

            //Seed
            Insert.IntoTable(nameof(User)).Row(new { GID = new Guid(), Username = "GJG-1" , Password = "123456"});
            Insert.IntoTable(nameof(User)).Row(new { GID = new Guid(), Username = "GJG-2" , Password = "123456"});
            Insert.IntoTable(nameof(User)).Row(new { GID = new Guid(), Username = "GJG-3" , Password = "123456"});
            Insert.IntoTable(nameof(User)).Row(new { GID = new Guid(), Username = "GJG-4" , Password = "123456"});

            Create.Table(nameof(Country)) //TODO: Seed with sample country
               .WithColumn("Id").AsInt32().PrimaryKey().Identity()
               .WithColumn("CountryName").AsString()
               .WithColumn("CountryIsoCode").AsString();

            //Seed
            Insert.IntoTable(nameof(Country)).Row(new { CountryName = "Turkey", CountryIsoCode = "TR" });
            Insert.IntoTable(nameof(Country)).Row(new { CountryName = "United States", CountryIsoCode = "US" });
            Insert.IntoTable(nameof(Country)).Row(new { CountryName = "United Kingdom", CountryIsoCode = "UK" });
            Insert.IntoTable(nameof(Country)).Row(new { CountryName = "France", CountryIsoCode = "FR" });

            Create.Table(nameof(Score))
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt32().Indexed().ForeignKey(nameof(User), "Id")
                .WithColumn("CountryId").AsInt32().ForeignKey(nameof(Country), "Id")
                .WithColumn("UserScore").AsInt32();


           
        }

        public override void Down()
        {
            Delete.Table(nameof(User));
            Delete.Table(nameof(Country));

            Delete.Table(nameof(Score));
        }
    }
}
