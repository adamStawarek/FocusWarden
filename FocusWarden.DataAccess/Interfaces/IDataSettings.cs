using FocusWarden.DataAccess.Models;
using System;

namespace FocusWarden.DataAccess.Interfaces
{
    public interface IDataSettings
    {
        public DataSet<TodoItem> TodoItems { get; set; }
        public DataSet<FocusSession> FocusSessions { get; set; }
        public TimeSpan SessionDuration { get; set; }
        public FilterSettings Settings { get; set; }
        public void Save();
    }
}