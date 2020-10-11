using AutoMapper.Configuration;
using Burak.GoodJobGames.Data;
using Burak.GoodJobGames.Data.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Burak.GoodJobGames.Business.Services.Implementation
{
    public class ScoreService
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;


        public ScoreService(DataContext dataContext,
              IConfiguration configuration)
        {
            _dataContext = dataContext;
            _configuration = configuration;
        }

        public async Task<Score> SubmitScore(Score score)
        {
            var result = _dataContext.Scores.Add(score);
            await _dataContext.SaveChangesAsync();

            return result.Entity;
        }
    }
}

