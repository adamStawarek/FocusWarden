using FocusWarden.DataAccess.Models;
using MediatR;
using System.Collections.Generic;

namespace FocusWarden.DataAccess.Domain.TodoItems.Query
{
    public class GetTodoItemsQuery : IRequest<IEnumerable<TodoItem>>
    {
        public Models.FilterSettings Settings { get; set; }
    }
}
