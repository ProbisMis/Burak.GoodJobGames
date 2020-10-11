using AutoMapper.Configuration;
using Burak.GoodJobGames.Business.Services.Interface;
using Burak.GoodJobGames.Data;
using Burak.GoodJobGames.Data.EntityModels;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Burak.GoodJobGames.Business.Services.Implementation
{
    public class ScoreService : IScoreService
    {
        private readonly DataContext _dataContext;


        public ScoreService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task SubmitScore(Score score)
        {
            //TODO: ID maybe problem for updating score
            var foundScore = GetScoreByUserId(score.UserId);
            if (foundScore == null)
            {
                _dataContext.Scores.Add(score);
            }
            else
            {
                foundScore.UserScore += score.UserScore;
                _dataContext.Scores.Update(foundScore);
            }
           
            await _dataContext.SaveChangesAsync();
        }

        private bool isExist(Score score)
        {
            var result = _dataContext.Scores.FirstOrDefault(x => x.UserId == score.UserId);
            if (result == null)
                return false;
            return true;
        }

        public Score GetScoreByUserId(Guid userId)
        {
            var result = _dataContext.Scores.FirstOrDefault(x => x.UserId == userId);
            return result;
        }
    }
}

