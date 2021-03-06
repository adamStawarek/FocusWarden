using FocusWarden.DataAccess.Models;
using MediatR;

namespace FocusWarden.DataAccess.Domain.TodoItems.Command
{
    public class AddTodoItemCommand : IRequest
    {
        public string Title { get; set; }
    }
}
