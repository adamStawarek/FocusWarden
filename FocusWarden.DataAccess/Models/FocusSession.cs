namespace FocusWarden.DataAccess.Models
{
    using Interfaces;
    using System;

    [Serializable]
    public class FocusSession : ISetEntity
    {
        public DateTime Date { get; set; }

        public TimeSpan FocusTime { get; set; }

        public bool IsCompleted { get; set; }
        public string Id { get; set; }
    }
}