using GoodJobGames.Models.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodJobGames.Data.EntityModels
{
    public class Score : IEntity<int>
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public int UserScore { get; set; }
    }
}
