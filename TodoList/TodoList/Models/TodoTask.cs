using System.ComponentModel.DataAnnotations;

namespace TodoList.Models
{
    public class TodoTask
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Text { get; set; }

        public bool IsDone { get; set; }

        [Required]
        public string OwnerId { get; set; }

        public User Owner { get; set; }

        public TodoTask()
        { }
    }
}