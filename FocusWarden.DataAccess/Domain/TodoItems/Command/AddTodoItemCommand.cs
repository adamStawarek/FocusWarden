namespace FocusWarden.DataAccess.Domain.TodoItems.Command
{
    using MediatR;

    public class AddTodoItemCommand : IRequest
    {
        public string Title { get; set; }
    }
}