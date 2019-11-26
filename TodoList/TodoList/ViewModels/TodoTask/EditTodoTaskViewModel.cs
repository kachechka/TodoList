using System.ComponentModel.DataAnnotations;

using Todo = TodoList.Models.TodoTask;

namespace TodoList.ViewModels.TodoTask
{
    public class EditTodoTaskViewModel
    {
        [Required]
        [MaxLength(150)]
        public string Text { get; set; }

        public bool IsDone { get; set; }

        public EditTodoTaskViewModel()
        { }

        public void ToModel(Todo todoTask)
        {
            todoTask.Text = Text;
            todoTask.IsDone = IsDone;
        }
    }
}