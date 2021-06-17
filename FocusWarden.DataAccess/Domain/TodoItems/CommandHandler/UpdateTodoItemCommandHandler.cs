namespace FocusWarden.DataAccess.Domain.TodoItems.CommandHandler
{
    using Command;
    using Interfaces;
    using MediatR;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class UpdateTodoItemCommandHandler : IRequestHandler<UpdateTodoItemCommand>
    {
        private readonly IDataSettings dataSettings;

        public UpdateTodoItemCommandHandler(IDataSettings dataSettings)
        {
            this.dataSettings = dataSettings;
        }

        public Task<Unit> Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
        {
            var todoItem = dataSettings.TodoItems.LocalSet.Single(t => t.Id == request.Id);
            todoItem.Title = request.Title ?? todoItem.Title;
            todoItem.ClosedAt = !todoItem.IsDone && request.IsDone.HasValue && request.IsDone.Value
                ? DateTime.Now
                : null;
            todoItem.IsDone = request.IsDone ?? todoItem.IsDone;
            dataSettings.TodoItems.LocalSet.Update(todoItem);
            dataSettings.Save();
            return Task.FromResult(new Unit());
        }
    }
}