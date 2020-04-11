using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quiztopia.Models.Repositories
{
    public interface IDifficultyRepo
    {
        Task<Difficulty> Add(Difficulty difficulty);
        Task<Difficulty> Delete(Difficulty difficulty);
        Task<IEnumerable<Difficulty>> GetAllDifficultiesAsync();
        Task<Difficulty> GetDifficultyByIdAsync(int difficultyId);
        Task<Difficulty> Update(Difficulty difficulty);
    }
}