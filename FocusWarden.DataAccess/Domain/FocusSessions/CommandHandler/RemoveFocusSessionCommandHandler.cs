namespace FocusWarden.DataAccess.Domain.FocusSessions.CommandHandler
{
    using Command;
    using Interfaces;
    using MediatR;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class RemoveFocusSessionCommandHandler : IRequestHandler<RemoveFocusSessionCommand>
    {
        private readonly IDataSettings dataSettings;

        public RemoveFocusSessionCommandHandler(IDataSettings dataSettings)
        {
            this.dataSettings = dataSettings;
        }

        public Task<Unit> Handle(RemoveFocusSessionCommand request, CancellationToken cancellationToken)
        {
            var session = dataSettings.FocusSessions.LocalSet.Single(s => s.Id == request.Id);
            dataSettings.FocusSessions.LocalSet.Remove(session);
            dataSettings.FocusSessions.DateTime = DateTime.Now;
            dataSettings.Save();

            return Task.FromResult(new Unit());
        }
    }
}