using FocusWarden.DataAccess.Domain.FocusSessions.Command;
using FocusWarden.DataAccess.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FocusWarden.DataAccess.Domain.FocusSessions.CommandHandler
{
    public class UpdateFocusSessionCommandHandler : IRequestHandler<UpdateFocusSessionCommand>
    {
        private readonly IDataSettings dataSettings;

        public UpdateFocusSessionCommandHandler(IDataSettings dataSettings)
        {
            this.dataSettings = dataSettings;
        }

        public Task<Unit> Handle(UpdateFocusSessionCommand request, CancellationToken cancellationToken)
        {
            dataSettings.FocusSessions.LocalSet.Update(new Models.FocusSession()
            {
                Id = request.Id,
                FocusTime = request.FocusTime,
                IsCompleted = request.IsCompleted,
                Date = DateTime.Now,
            });
            dataSettings.Save();
            return Task.FromResult(new Unit());
        }
    }
}
