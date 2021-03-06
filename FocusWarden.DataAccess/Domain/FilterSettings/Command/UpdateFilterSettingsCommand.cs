using MediatR;

namespace FocusWarden.DataAccess.Domain.FilterSettings.Command
{
    public class UpdateFilterSettingsCommand : IRequest
    {
        public Models.FilterSettings Settings { get; set; }
    }
}