namespace Burak.GoodJobGames.Models.BaseModel
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}
