using GoodJobGames.Data.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodJobGames.Business.Services.Interface
{
    public interface ICountryService
    {
        Task<Country> GetCountry(int countryId);
        Task<List<Country>> GetAllCountry();
        Task<Country> GetCountryByIsoCode(string countryId);
    }
}
