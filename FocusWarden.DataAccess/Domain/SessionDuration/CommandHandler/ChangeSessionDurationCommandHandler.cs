using FocusWarden.DataAccess.Domain.SessionDuration.Command;
using FocusWarden.DataAccess.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FocusWarden.DataAccess.Domain.SessionDuration.CommandHandler
{
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
                Common.Enumerators.AtomicOperationType.Increase => dataSettings.SessionDuration.Add(TimeSpan.FromMinutes(5)),
                Common.Enumerators.AtomicOperationType.Decrease => dataSettings.SessionDuration.Subtract(TimeSpan.FromMinutes(5)),
                _ => dataSettings.SessionDuration,
            };
            dataSettings.Save();

            return Task.FromResult(dataSettings.SessionDuration);
        }
    }
}
