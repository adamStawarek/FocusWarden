namespace FocusWarden.DataAccess.Domain.SessionDuration.QueryHandler
{
    using Interfaces;
    using MediatR;
    using Query;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

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