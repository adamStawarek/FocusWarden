using FocusWarden.DataAccess.Domain.SessionDuration.Query;
using FocusWarden.DataAccess.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FocusWarden.DataAccess.Domain.SessionDuration.QueryHandler
{
    public class GetSessionDurationQueryHandler : IRequestHandler<GetSessionDurationQuery, TimeSpan>
    {
        private readonly IDataSettings dataSettings;

        public GetSessionDurationQueryHandler(IDataSettings dataSettings)
        {
            this.dataSettings = dataSettings;
        }

        public Task<TimeSpan> Handle(GetSessionDurationQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(dataSettings.SessionDuration);
        }
    }
}
