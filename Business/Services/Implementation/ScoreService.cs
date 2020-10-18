using GoodJobGames.Business.Services.Interface;
using GoodJobGames.Data;
using GoodJobGames.Data.EntityModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodJobGames.Business.Services.Implementation
{
    public class ScoreService : IScoreService
    {
        private readonly DataContext _dataContext;

        public ScoreService(DataContext dataContext
            )
        {
            _dataContext = dataContext;
        }

        public async Task SubmitScore(UserScore score)
        {
            var foundScore = GetScoreByUserId(score.UserId);
            if (foundScore == null)
            {
                _dataContext.Scores.Add(score);
            }
            else
            {
                foundScore.Score += score.Score;
                _dataContext.Scores.Update(foundScore);
            }
           
            await _dataContext.SaveChangesAsync();
        }

        public async Task<List<UserScore>> GetScores(int numberOfRecords)
        {

            var scores = _dataContext.Scores.Include(x=> x.User.Country).OrderByDescending(x => x.Score).Take(numberOfRecords);
            return scores.ToList();
        }

        public UserScore GetScoreByUserId(Guid userId)
        {
            var result = _dataContext.Scores.FirstOrDefault(x => x.UserId == userId);
            return result;
        }
    }
}

