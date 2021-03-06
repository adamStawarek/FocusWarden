using MediatR;

namespace FocusWarden.DataAccess.Domain.TodoItems.Command
{
    public class RemoveTodoItemCommand : IRequest
    {
        public string Id { get; set; }
    }

}
