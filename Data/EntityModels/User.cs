﻿using GoodJobGames.Models.BaseModel;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodJobGames.Data.EntityModels
{
    /// <summary>
    /// User Migration DB Entity class
    /// </summary>
    public class User : IEntity<int>
    {
        public int Id { get; set; }
        public Guid GID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } 
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string Token { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime UpdatedOnUtc { get; set; }
        public UserScore Score { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
    }
}
