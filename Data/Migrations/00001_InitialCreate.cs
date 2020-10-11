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
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("GID").AsGuid().NotNullable()
                .WithColumn("Username").AsString().Nullable()
                .WithColumn("Password").AsString().Nullable()
                .WithColumn("Email").AsString().Nullable()
                .WithColumn("IsActive").AsBoolean().Nullable()
                .WithColumn("IsDeleted").AsBoolean().Nullable()
                .WithColumn("CreatedOnUtc").AsDateTime().Nullable()
                .WithColumn("UpdatedOnUtc").AsDateTime().Nullable()
                .WithColumn("Token").AsString().Nullable();

            Create.Table(nameof(Score))
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt32().Indexed()
                .WithColumn("UserScore").AsInt32();
        }

        public override void Down()
        {
            Delete.Table(nameof(User));
            Delete.Table(nameof(Score));
        }
    }
}
