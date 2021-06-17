namespace FocusWarden.DataAccess.Domain.SessionDuration.CommandHandler
{
    using Command;
    using Common.Enumerators;
    using Interfaces;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class ChangeSessionDurationCommandHandler : IRequestHandler<ChangeSessionDurationCommand, TimeSpan>
    {
        private readonly IDataSettings dataSettings;

        public ChangeSessionDurationCommandHandler(IDataSettings dataSettings)
        {
            this.dataSettings = dataSettings;
        }

        public Task<TimeSpan> Handle(ChangeSessionDurationCommand request, CancellationToken cancellationToken)
        {
            dataSettings.SessionDuration = request.Type switch
            {
                AtomicOperationType.Increase => dataSettings.SessionDuration.Add(TimeSpan.FromMinutes(5)),
                AtomicOperationType.Decrease => dataSettings.SessionDuration.Subtract(TimeSpan.FromMinutes(5)),
                _ => dataSettings.SessionDuration
            };
            dataSettings.Save();

            return Task.FromResult(dataSettings.SessionDuration);
        }
    }
}