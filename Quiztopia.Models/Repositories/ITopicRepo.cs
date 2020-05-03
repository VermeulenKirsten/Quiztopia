using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quiztopia.Models.Repositories
{
    public interface ITopicRepo
    {
        Task<Topic> Add(Topic topic);
        Task<Topic> Delete(Topic topic);
        Task<IEnumerable<Topic>> GetAllTopicsAsync();
        Task<Topic> GetTopicByIdAsync(int topicId);
        Task<Topic> GetTopicByNameAsync(string name);
        Task<Topic> Update(Topic topic);
    }
}