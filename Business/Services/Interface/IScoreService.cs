using GoodJobGames.Data.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodJobGames.Business.Services.Interface
{
    public interface IScoreService
    {
        Task SubmitScore(UserScore score);
        UserScore GetScoreByUserId(Guid userId);
        Task<List<UserScore>> GetScores(int numberOfRecords);
    }
}
