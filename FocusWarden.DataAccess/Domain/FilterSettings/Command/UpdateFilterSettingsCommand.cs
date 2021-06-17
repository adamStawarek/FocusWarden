namespace FocusWarden.DataAccess.Domain.FilterSettings.Command
{
    using MediatR;
    using Models;

    public class UpdateFilterSettingsCommand : IRequest
    {
        public FilterSettings Settings { get; set; }
    }
}