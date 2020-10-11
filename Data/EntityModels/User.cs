using Burak.GoodJobGames.Models.BaseModel;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Burak.GoodJobGames.Data.EntityModels
{
    /// <summary>
    /// User Migration DB Entity class
    /// </summary>
    public class User : IEntity<Guid>, ISoftDelete
    {
        public Guid Id { get; set; }
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
