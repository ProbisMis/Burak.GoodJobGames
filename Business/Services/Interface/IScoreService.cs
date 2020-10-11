using Burak.GoodJobGames.Data.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Burak.GoodJobGames.Business.Services.Interface
{
    public interface IScoreService
    {
        Task<Score> SubmitScore(Score score);
    }
}
