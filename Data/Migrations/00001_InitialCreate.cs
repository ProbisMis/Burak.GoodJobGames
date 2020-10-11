using FluentMigrator;
using Burak.GoodJobGames.Data.EntityModels;

namespace Burak.GoodJobGames.Data.Migrations
{
    [Migration(1)]
    public partial class _00001_InitialCreate : Migration
    {
        public override void Up()
        {
            Create.Table(nameof(User))
                .WithColumn("Id").AsGuid().PrimaryKey().Identity()
                .WithColumn("Username").AsString().Nullable()
                .WithColumn("Password").AsString().Nullable()
                .WithColumn("Email").AsString().Nullable()
                .WithColumn("IsActive").AsBoolean().Nullable()
                .WithColumn("IsDeleted").AsBoolean().Nullable()
                .WithColumn("CreatedOnUtc").AsDateTime().Nullable()
                .WithColumn("UpdatedOnUtc").AsDateTime().Nullable()
                .WithColumn("Token").AsString().Nullable();
        }

        public override void Down()
        {
            Delete.Table(nameof(User));
        }
    }
}
