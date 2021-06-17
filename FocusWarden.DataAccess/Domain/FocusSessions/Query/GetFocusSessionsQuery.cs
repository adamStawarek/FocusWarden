namespace FocusWarden.DataAccess.Domain.FocusSessions.Query
{
    using MediatR;
    using Models;
    using System;
    using System.Collections.Generic;

    public class GetFocusSessionsQuery : IRequest<IEnumerable<FocusSession>>
    {
        public DateTime? Date { get; set; }
        public bool? Completed { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}