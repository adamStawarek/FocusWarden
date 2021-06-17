namespace FocusWarden.DataAccess.Interfaces
{
    using Models;
    using System;

    public interface IDataSettings
    {
        public DataSet<TodoItem> TodoItems { get; set; }
        public DataSet<FocusSession> FocusSessions { get; set; }
        public TimeSpan SessionDuration { get; set; }
        public FilterSettings Settings { get; set; }
        public void Save();
    }
}