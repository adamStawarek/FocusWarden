namespace FocusWarden.DataAccess.Domain.TodoItems.Query
{
    using MediatR;
    using Models;
    using System.Collections.Generic;

    public class GetTodoItemsQuery : IRequest<IEnumerable<TodoItem>>
    {
        public FilterSettings Settings { get; set; }
    }
}