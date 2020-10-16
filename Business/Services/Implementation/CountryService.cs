using GoodJobGames.Business.Services.Interface;
using GoodJobGames.Data;
using GoodJobGames.Data.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodJobGames.Business.Services.Implementation
{
    public class CountryService : ICountryService
    {
        private readonly DataContext _dataContext;
        //private readonly ICacheService _cacheService;


        public CountryService(DataContext dataContext
            //ICacheService cacheService
            )
        {
            _dataContext = dataContext;
            //_cacheService = cacheService;
        }

        public async Task<Country> GetCountry(int countryId)
        {
           return _dataContext.Countries.FirstOrDefault(x => x.Id == countryId);
        }

        public async Task<List<Country>> GetAllCountry()
        {
            return _dataContext.Countries.ToList();
        }

        public async Task<Country> GetCountryByIsoCode(string countryIsoCode)
        {
            return _dataContext.Countries.FirstOrDefault(x => x.CountryIsoCode == countryIsoCode);
        }
    }
}
