using MediatR;

namespace FocusWarden.DataAccess.Domain.FilterSettings.Query
{
    public class GetFilterSettingsQuery : IRequest<Models.FilterSettings>
    {
    }
}
