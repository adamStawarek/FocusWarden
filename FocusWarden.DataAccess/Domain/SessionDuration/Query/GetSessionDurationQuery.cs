namespace FocusWarden.DataAccess.Domain.SessionDuration.Query
{
    using MediatR;
    using System;

    public class GetSessionDurationQuery : IRequest<TimeSpan>
    {
    }
}