using FocusWarden.DataAccess.Domain.FilterSettings.Query;
using FocusWarden.DataAccess.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FocusWarden.DataAccess.Domain.FilterSettings.QueryHandler
{
    public class GetFilterSettingsQueryHandler : IRequestHandler<GetFilterSettingsQuery, Models.FilterSettings>
    {
        private readonly IDataSettings dataSettings;

        public GetFilterSettingsQueryHandler(IDataSettings dataSettings)
        {
            this.dataSettings = dataSettings;
        }

        public Task<Models.FilterSettings> Handle(GetFilterSettingsQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(dataSettings.Settings);
        }
    }
}
