using MediatR;

namespace FocusWarden.DataAccess.Domain.TodoItems.Command
{
    public class UpdateTodoItemCommand : IRequest
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public bool? IsDone { get; set; }
    }
}
