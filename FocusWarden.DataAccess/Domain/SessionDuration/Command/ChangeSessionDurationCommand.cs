using FocusWarden.Common.Enumerators;
using MediatR;
using System;

namespace FocusWarden.DataAccess.Domain.SessionDuration.Command
{
    public class ChangeSessionDurationCommand : IRequest<TimeSpan>
    {
        public AtomicOperationType Type { get; set; }
    }
}
