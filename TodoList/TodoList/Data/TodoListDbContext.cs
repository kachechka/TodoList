using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoList.Models;

namespace TodoList.Data
{
    public class TodoListDbContext : IdentityDbContext<User>
    {
        public DbSet<TodoTask> TodoTasks { get; set; }

        public TodoListDbContext(DbContextOptions options) : base(options)
            => Database.EnsureCreated();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TodoTask>()
                .HasOne(e => e.Owner)
                .WithMany(e => e.TodoTasks)
                .HasForeignKey(e => e.OwnerId)
                .IsRequired();
        }
    }
}