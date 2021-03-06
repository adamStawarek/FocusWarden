﻿using FocusWarden.DataAccess.Domain.FocusSessions.Query;
using FocusWarden.DataAccess.Interfaces;
using FocusWarden.DataAccess.Models;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FocusWarden.DataAccess.Domain.FocusSessions.QueryHandler
{

    public class GetFocusSessionsQueryHandler : IRequestHandler<GetFocusSessionsQuery, IEnumerable<FocusSession>>
    {
        private readonly IDataSettings dataSettings;

        public GetFocusSessionsQueryHandler(IDataSettings dataSettings)
        {
            this.dataSettings = dataSettings;
        }

        public Task<IEnumerable<FocusSession>> Handle(GetFocusSessionsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<FocusSession> focusSessions = dataSettings.FocusSessions.LocalSet;

            if (request.Date.HasValue)
            {
                focusSessions = focusSessions.Where(s => s.Date.Date.Equals(request.Date.Value.Date));
            }

            if (request.Completed.HasValue)
            {
                focusSessions = focusSessions.Where(s => s.IsCompleted == request.Completed);
            }

            if(request.FromDate.HasValue && !request.Date.HasValue)
            {
                focusSessions = focusSessions.Where(s => s.Date.Date >= request.FromDate.Value.Date);
            }

            if (request.ToDate.HasValue && !request.Date.HasValue)
            {
                focusSessions = focusSessions.Where(s => s.Date.Date <= request.ToDate.Value.Date);
            }

            return Task.FromResult(focusSessions);
        }
    }

}
