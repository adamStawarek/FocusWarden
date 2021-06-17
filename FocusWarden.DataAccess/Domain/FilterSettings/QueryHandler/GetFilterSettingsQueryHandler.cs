namespace FocusWarden.DataAccess.Domain.FilterSettings.QueryHandler
{
    using Interfaces;
    using MediatR;
    using Models;
    using Query;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetFilterSettingsQueryHandler : IRequestHandler<GetFilterSettingsQuery, FilterSettings>
    {
        private readonly IDataSettings dataSettings;

        public GetFilterSettingsQueryHandler(IDataSettings dataSettings)
        {
            this.dataSettings = dataSettings;
        }

        public Task<FilterSettings> Handle(GetFilterSettingsQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(dataSettings.Settings);
        }
    }
}