using Microsoft.EntityFrameworkCore;
using Quiztopia.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiztopia.Models.Repositories
{
    public class DifficultyRepo : IDifficultyRepo
    {
        private readonly QuiztopiaDbContext context;

        public DifficultyRepo(QuiztopiaDbContext context)
        {
            this.context = context;
        }

        public async Task<Difficulty> Add(Difficulty difficulty)
        {
            try
            {
                var result = context.Difficulties.AddAsync(difficulty);
                await context.SaveChangesAsync();
                return difficulty;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<Difficulty> Update(Difficulty difficulty)
        {
            try
            {
                context.Difficulties.Update(difficulty);
                await context.SaveChangesAsync();
                return difficulty;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<Difficulty> Delete(Difficulty difficulty)
        {
            try
            {
                context.Difficulties.Remove(difficulty);
                await context.SaveChangesAsync();
                return difficulty;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<Difficulty>> GetAllDifficultiesAsync()
        {
            return await context.Difficulties.OrderBy(e => e.Name).ToListAsync();
        }

        public async Task<Difficulty> GetDifficultyByIdAsync(int difficultyId)
        {
            return await context.Difficulties.SingleOrDefaultAsync<Difficulty>(e => e.Id == difficultyId);
        }

        public async Task<Difficulty> GetDifficultyByNameAsync(string name)
        {
            return await context.Difficulties.SingleOrDefaultAsync<Difficulty>(e => e.Name == name);
        }
    }
}
