using System.ComponentModel.DataAnnotations;

using Todo = TodoList.Models.TodoTask;

namespace TodoList.ViewModels.TodoTask
{
    public class AddTodoTaskViewModel
    {
        [Required]
        [MaxLength(150)]
        public string Text { get; set; }

        public AddTodoTaskViewModel()
        { }

        public Todo ToModel()
        {
            var model = new Todo
            {
                Text = Text
            };

            return model;
        }
    }
}