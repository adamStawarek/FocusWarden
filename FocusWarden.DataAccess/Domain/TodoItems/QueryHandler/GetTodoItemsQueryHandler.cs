using FocusWarden.Common.Enumerators;
using FocusWarden.DataAccess.Domain.TodoItems.Query;
using FocusWarden.DataAccess.Interfaces;
using FocusWarden.DataAccess.Models;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FocusWarden.DataAccess.Domain.FilterSettings.Query;

namespace FocusWarden.DataAccess.Domain.TodoItems.QueryHandler
{
    public class GetTodoItemsQueryHandler : IRequestHandler<GetTodoItemsQuery, IEnumerable<TodoItem>>
    {
        private readonly IDataSettings dataSettings;
        private readonly IMediator mediator;

        public GetTodoItemsQueryHandler(IDataSettings dataSettings, IMediator mediator)
        {
            this.dataSettings = dataSettings;
            this.mediator = mediator;
        }

        public async Task<IEnumerable<TodoItem>> Handle(GetTodoItemsQuery request, CancellationToken cancellationToken)
        {
            var todoItems = new List<TodoItem>();

            var settings = request.Settings ?? 
                           await mediator.Send(new GetFilterSettingsQuery(), cancellationToken);

            if (settings.Status.IsChecked)
            {
                todoItems = todoItems.Union(dataSettings.TodoItems.LocalSet
                    .Where(i => settings.Status.Value.Equals(TodoItemStatus.Open) ? !i.IsDone : i.IsDone))
                    .ToList();
            }

            if (settings.CreatedAt.IsChecked)
            {
                todoItems = todoItems.Union(dataSettings.TodoItems.LocalSet
                    .Where(i => i.CreatedAt.Date.Equals(settings.CreatedAt.Value.Date)))
                    .ToList();
            }

            if (settings.ClosedAt.IsChecked)
            {
                todoItems = todoItems.Union(dataSettings.TodoItems.LocalSet
                    .Where(i => i.ClosedAt.HasValue && i.ClosedAt.Value.Date.Equals(settings.ClosedAt.Value.Date)))
                    .ToList();
            }

            return todoItems;
        }
    }
}
