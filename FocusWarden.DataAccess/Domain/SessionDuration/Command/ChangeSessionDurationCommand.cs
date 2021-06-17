namespace FocusWarden.DataAccess.Domain.SessionDuration.Command
{
    using Common.Enumerators;
    using MediatR;
    using System;

    public class ChangeSessionDurationCommand : IRequest<TimeSpan>
    {
        public AtomicOperationType Type { get; set; }
    }
}