using FocusWarden.DataAccess.Domain.FocusSessions.Command;
using FocusWarden.DataAccess.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FocusWarden.DataAccess.Domain.FocusSessions.CommandHandler
{
    public class AddFocusSessionCommandHandler : IRequestHandler<AddFocusSessionCommand, string>
    {
        private readonly IDataSettings dataSettings;

        public AddFocusSessionCommandHandler(IDataSettings dataSettings)
        {
            this.dataSettings = dataSettings;
        }

        public Task<string> Handle(AddFocusSessionCommand request, CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid().ToString();
            dataSettings.FocusSessions.LocalSet.Add(new Models.FocusSession()
            {
                Id = id,
                Date = DateTime.Now,
                FocusTime = TimeSpan.FromMinutes(0)
            });
            dataSettings.Save();
            return Task.FromResult(id);
        }
    }
}
