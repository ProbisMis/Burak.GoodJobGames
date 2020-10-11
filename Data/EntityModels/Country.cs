using Burak.GoodJobGames.Models.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Burak.GoodJobGames.Data.EntityModels
{
    public class Country : IEntity<int>
    {
        public int Id { get; set; }
        public int CountryName { get; set; }
        public int CountryIsoCode { get; set; }
    }
}
