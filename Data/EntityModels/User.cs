using Burak.GoodJobGames.Models.BaseModel;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Burak.GoodJobGames.Data.EntityModels
{
    /// <summary>
    /// User Migration DB Entity class
    /// </summary>
    public class User : IEntity<int>, ISoftDelete
    {
        public int Id { get; set; }
        public Guid GID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } 
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }

        public DateTime CreatedOnUtc { get; set; }
        public DateTime UpdatedOnUtc { get; set; }
    }
}
