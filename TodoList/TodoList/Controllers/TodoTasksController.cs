using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoList.Data.Repositories;
using TodoList.Filters;
using TodoList.Models;
using TodoList.ViewModels.TodoTask;

namespace TodoList.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TodoTasksController : ControllerBase
    {
        private readonly ITodoTaskRepository _repository;
        private readonly Lazy<string> _currentUserId;

        public TodoTasksController(
            ITodoTaskRepository repository,
            UserManager<User> userManager)
        {
            _repository = repository;
            _currentUserId = new Lazy<string>(
                () => userManager.GetUserId(User));
        }

        [HttpGet("{id}")]
        public async Task<TodoTaskViewModel> Get(string id)
        {
            var todoTask = await _repository.GetAsync(id);

            return todoTask is null 
                ? null 
                : TodoTaskViewModel.FromModel(todoTask);
        }

        [HttpGet]
        public async Task<IEnumerable<TodoTaskViewModel>> GetAll()
        {
            var todoTasks = 
                await _repository.GetAllAsync(_currentUserId.Value);

            return todoTasks.ConvertAll(TodoTaskViewModel.FromModel);
        }

        [HttpGet("Active")]
        public async Task<IEnumerable<TodoTaskViewModel>> GetAllActive()
        {
            var todoTasks = 
                await _repository.GetAllAsync(_currentUserId.Value, false);

            return todoTasks.ConvertAll(TodoTaskViewModel.FromModel);
        }

        [HttpGet("Done")]
        public async Task<IEnumerable<TodoTaskViewModel>> GetAllDone()
        {
            var todoTasks =
                await _repository.GetAllAsync(_currentUserId.Value, true);

            return todoTasks.ConvertAll(TodoTaskViewModel.FromModel);
        }

        [HttpPost]
        [ValidModelStateFilter]
        public async Task<IActionResult> Add(
            AddTodoTaskViewModel viewModel)
        {
            var todoTask = viewModel.ToModel();
            todoTask.OwnerId = _currentUserId.Value;

            await _repository.AddAsync(todoTask);

            return CreatedAtAction(
                nameof(Get), 
                new { id = todoTask.Id }, 
                TodoTaskViewModel.FromModel(todoTask));
        }

        [HttpPut("{id}")]
        [ValidModelStateFilter]
        public async Task<IActionResult> Edit(
            string id,
            EditTodoTaskViewModel viewModel)
        {
            var todoTask = await _repository.GetAsync(id);

            if (todoTask is null)
            {
                return NotFound();
            }

            if (!todoTask.OwnerId.Equals(_currentUserId.Value))
            {
                return Forbid();
            }

            viewModel.ToModel(todoTask);

            await _repository.UpdateAsync(todoTask);

            return Ok(todoTask);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var todoTask = await _repository.GetAsync(id);

            if (todoTask is null)
            {
                return NotFound();
            }

            if (!todoTask.OwnerId.Equals(_currentUserId.Value))
            {
                return Forbid();
            }

            await _repository.DeleteAsync(todoTask);

            return new EmptyResult();
        }
    }
}