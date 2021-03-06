using MediatR;

namespace FocusWarden.DataAccess.Domain.FocusSessions.Command
{
    public class RemoveFocusSessionCommand : IRequest
    {
        public string Id { get; set; }
    }
}
