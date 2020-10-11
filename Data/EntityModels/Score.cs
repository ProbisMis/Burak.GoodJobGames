using Burak.GoodJobGames.Models.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Burak.GoodJobGames.Data.EntityModels
{
    public class Score : IEntity<int>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int UserScore { get; set; }
    }
}
