using MediatR;
using System;

namespace FocusWarden.DataAccess.Domain.FocusSessions.Command
{
    public class UpdateFocusSessionCommand : IRequest
    {
        public string Id { get; set; }
        public TimeSpan FocusTime { get; set; }
        public bool IsCompleted { get; set; }
    }
}
