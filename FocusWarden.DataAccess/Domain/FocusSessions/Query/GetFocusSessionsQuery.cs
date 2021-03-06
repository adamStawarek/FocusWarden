using FocusWarden.DataAccess.Models;
using MediatR;
using System;
using System.Collections.Generic;

namespace FocusWarden.DataAccess.Domain.FocusSessions.Query
{
    public class GetFocusSessionsQuery : IRequest<IEnumerable<FocusSession>>
    {
        public DateTime? Date { get; set; }
        public bool? Completed { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
