namespace FocusWarden.DataAccess.Domain.TodoItems.CommandHandler
{
    using Command;
    using Interfaces;
    using MediatR;
    using Models;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class AddTodoItemCommandHandler : IRequestHandler<AddTodoItemCommand>
    {
        private readonly IDataSettings dataSettings;

        public AddTodoItemCommandHandler(IDataSettings dataSettings)
        {
            this.dataSettings = dataSettings;
        }

        public Task<Unit> Handle(AddTodoItemCommand request, CancellationToken cancellationToken)
        {
            dataSettings.TodoItems.LocalSet.Add(
                new TodoItem {Id = Guid.NewGuid().ToString(), CreatedAt = DateTime.Now, Title = request.Title});
            dataSettings.Save();
            return Task.FromResult(new Unit());
        }
    }
}