namespace Burak.GoodJobGames.Models.BaseModel
{
    public interface IEntity<T> where T : struct
    {
        T Id { get; set; }
    }
}
