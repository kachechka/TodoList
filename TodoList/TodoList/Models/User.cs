using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TodoList.Models
{
    public class User : IdentityUser
    {
        public List<TodoTask> TodoTasks { get; set; }

        public User()
        { }
    }
}