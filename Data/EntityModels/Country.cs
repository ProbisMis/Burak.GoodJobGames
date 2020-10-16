using GoodJobGames.Models.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodJobGames.Data.EntityModels
{
    public class Country : IEntity<int>
    {
        public int Id { get; set; }
        public string CountryName { get; set; }
        public string CountryIsoCode { get; set; }
        
        public User User { get; set; }
    }
}
