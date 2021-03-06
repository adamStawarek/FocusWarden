using FocusWarden.DataAccess.Domain.TodoItems.Command;
using FocusWarden.DataAccess.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FocusWarden.DataAccess.Domain.TodoItems.CommandHandler
{
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
                new Models.TodoItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Now,
                    Title = request.Title
                });
            dataSettings.Save();
            return Task.FromResult(new Unit());
        }
    }
}
