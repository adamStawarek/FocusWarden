namespace FocusWarden.DataAccess.Domain.TodoItems.Command
{
    using MediatR;

    public class RemoveTodoItemCommand : IRequest
    {
        public string Id { get; set; }
    }
}