using FocusWarden.DataAccess.Domain.FilterSettings.Command;
using FocusWarden.DataAccess.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FocusWarden.DataAccess.Domain.FilterSettings.CommandHandler
{
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
