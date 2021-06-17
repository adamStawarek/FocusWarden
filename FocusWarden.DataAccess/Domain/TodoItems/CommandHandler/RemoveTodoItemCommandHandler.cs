namespace FocusWarden.DataAccess.Domain.TodoItems.CommandHandler
{
    using Command;
    using Interfaces;
    using MediatR;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class RemoveTodoItemCommandHandler : IRequestHandler<RemoveTodoItemCommand>
    {
        private readonly IDataSettings dataSettings;

        public RemoveTodoItemCommandHandler(IDataSettings dataSettings)
        {
            this.dataSettings = dataSettings;
        }

        public Task<Unit> Handle(RemoveTodoItemCommand request, CancellationToken cancellationToken)
        {
            var todoItem = dataSettings.TodoItems.LocalSet.Single(t => t.Id == request.Id);
            dataSettings.TodoItems.LocalSet.Remove(todoItem);
            dataSettings.Save();
            return Task.FromResult(new Unit());
        }
    }
}