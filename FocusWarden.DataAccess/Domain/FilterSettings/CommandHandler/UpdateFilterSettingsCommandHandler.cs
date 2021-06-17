namespace FocusWarden.DataAccess.Domain.FilterSettings.CommandHandler
{
    using Command;
    using Interfaces;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class UpdateFilterSettingsCommandHandler : IRequestHandler<UpdateFilterSettingsCommand>
    {
        private readonly IDataSettings dataSettings;

        public UpdateFilterSettingsCommandHandler(IDataSettings dataSettings)
        {
            this.dataSettings = dataSettings;
        }

        public Task<Unit> Handle(UpdateFilterSettingsCommand request, CancellationToken cancellationToken)
        {
            dataSettings.Settings = request.Settings;
            return Task.FromResult(new Unit());
        }
    }
}