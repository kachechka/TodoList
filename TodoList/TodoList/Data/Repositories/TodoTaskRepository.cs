using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Models;

namespace TodoList.Data.Repositories
{
    public class TodoTaskRepository : ITodoTaskRepository
    {
        private readonly TodoListDbContext _db;

        public TodoTaskRepository(TodoListDbContext db) => _db = db;

        public Task<List<TodoTask>> GetAllAsync(string ownerId)
            => GetAll(ownerId).ToListAsync();

        public Task<List<TodoTask>> GetAllAsync(string ownerId, bool isDone)
            => GetAll(ownerId)
                .Where(t => t.IsDone == isDone)
                .ToListAsync();

        private IQueryable<TodoTask> GetAll(string ownerId)
            => _db.TodoTasks.Where(t => t.OwnerId.Equals(ownerId));

        public Task AddAsync(TodoTask todoTask)
        {
            _db.TodoTasks.Add(todoTask);

            return _db.SaveChangesAsync();
        }

        public Task<TodoTask> GetAsync(string id) 
            => _db.TodoTasks.FirstOrDefaultAsync(t => t.Id.Equals(id));

        public Task UpdateAsync(TodoTask todoTask)
        {
            _db.TodoTasks.Update(todoTask);

            return _db.SaveChangesAsync();
        }

        public Task DeleteAsync(TodoTask todoTask)
        {
            _db.TodoTasks.Remove(todoTask);

            return _db.SaveChangesAsync();
        }
    }
}