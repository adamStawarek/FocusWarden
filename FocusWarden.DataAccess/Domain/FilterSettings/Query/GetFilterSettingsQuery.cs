namespace FocusWarden.DataAccess.Domain.FilterSettings.Query
{
    using MediatR;
    using Models;

    public class GetFilterSettingsQuery : IRequest<FilterSettings>
    {
    }
}