namespace FocusWarden.DataAccess.Domain.FocusSessions.CommandHandler
{
    using Command;
    using Interfaces;
    using MediatR;
    using Models;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

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
            dataSettings.FocusSessions.LocalSet.Add(new FocusSession
            {
                Id = id, Date = DateTime.Now, FocusTime = TimeSpan.FromMinutes(0)
            });
            dataSettings.Save();
            return Task.FromResult(id);
        }
    }
}