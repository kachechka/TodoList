using Todo = TodoList.Models.TodoTask;

namespace TodoList.ViewModels.TodoTask
{
    public class TodoTaskViewModel
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public bool IsDone { get; set; }

        public TodoTaskViewModel()
        { }

        public static TodoTaskViewModel FromModel(Todo todoTask)
        {
            var viewModel = new TodoTaskViewModel
            {
                Id = todoTask.Id,
                Text = todoTask.Text,
                IsDone = todoTask.IsDone
            };

            return viewModel;
        }
    }
}