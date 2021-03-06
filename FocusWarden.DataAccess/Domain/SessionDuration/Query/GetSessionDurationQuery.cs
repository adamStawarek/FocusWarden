using MediatR;
using System;

namespace FocusWarden.DataAccess.Domain.SessionDuration.Query
{
    public class GetSessionDurationQuery : IRequest<TimeSpan>
    {
    }
}
