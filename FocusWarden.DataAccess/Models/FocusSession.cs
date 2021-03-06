using FocusWarden.DataAccess.Interfaces;
using System;

namespace FocusWarden.DataAccess.Models
{
    [Serializable]
    public class FocusSession : ISetEntity
    {
        public string Id { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan FocusTime { get; set; }

        public bool IsCompleted { get; set; }
    }
}