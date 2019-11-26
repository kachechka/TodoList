using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Models;

namespace TodoList.Data.Repositories
{
    public interface ITodoTaskRepository
    {
        Task<List<TodoTask>> GetAllAsync(string ownerId);

        Task<List<TodoTask>> GetAllAsync(string ownerId, bool isDone);

        Task AddAsync(TodoTask todoTask);

        Task<TodoTask> GetAsync(string id);

        Task UpdateAsync(TodoTask todoTask);

        Task DeleteAsync(TodoTask todoTask);
    }
}