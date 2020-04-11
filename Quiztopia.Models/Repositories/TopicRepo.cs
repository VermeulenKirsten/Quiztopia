using Microsoft.EntityFrameworkCore;
using Quiztopia.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiztopia.Models.Repositories
{
    public class TopicRepo : ITopicRepo
    {
        private readonly QuiztopiaDbContext context;

        public TopicRepo(QuiztopiaDbContext context)
        {
            this.context = context;
        }

        public async Task<Topic> Add(Topic topic)
        {
            try
            {
                var result = context.Topics.AddAsync(topic);
                await context.SaveChangesAsync();
                return topic;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<Topic> Update(Topic topic)
        {
            try
            {
                context.Topics.Update(topic);
                await context.SaveChangesAsync();
                return topic;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<Topic> Delete(Topic topic)
        {
            try
            {
                context.Topics.Remove(topic);
                await context.SaveChangesAsync();
                return topic;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<Topic>> GetAllTopicsAsync()
        {
            return await context.Topics.OrderBy(e => e.Name).ToListAsync();
        }

        public async Task<Topic> GetTopicByIdAsync(int topicId)
        {
            return await context.Topics.SingleOrDefaultAsync<Topic>(e => e.Id == topicId);
        }
    }
}
