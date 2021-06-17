namespace FocusWarden.DataAccess.Domain.FocusSessions.CommandHandler
{
    using Command;
    using Interfaces;
    using MediatR;
    using Models;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class UpdateFocusSessionCommandHandler : IRequestHandler<UpdateFocusSessionCommand>
    {
        private readonly IDataSettings dataSettings;

        public UpdateFocusSessionCommandHandler(IDataSettings dataSettings)
        {
            this.dataSettings = dataSettings;
        }

        public Task<Unit> Handle(UpdateFocusSessionCommand request, CancellationToken cancellationToken)
        {
            dataSettings.FocusSessions.LocalSet.Update(new FocusSession
            {
                Id = request.Id,
                FocusTime = request.FocusTime,
                IsCompleted = request.IsCompleted,
                Date = DateTime.Now
            });
            dataSettings.Save();
            return Task.FromResult(new Unit());
        }
    }
}