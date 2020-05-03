using System.Threading.Tasks;

namespace Quiztopia.Models.Repositories
{
    public interface IScoreboardRepo
    {
        Task<Scoreboard> Add(Scoreboard scoreboard);
        Task<QuizzesScoreboards> AddScoreToQuiz(QuizzesScoreboards quizzesScoreboards);
    }
}