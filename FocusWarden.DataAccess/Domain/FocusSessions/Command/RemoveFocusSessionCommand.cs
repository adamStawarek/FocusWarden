namespace FocusWarden.DataAccess.Domain.FocusSessions.Command
{
    using MediatR;

    public class RemoveFocusSessionCommand : IRequest
    {
        public string Id { get; set; }
    }
}