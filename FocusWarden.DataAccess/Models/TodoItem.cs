namespace FocusWarden.DataAccess.Models
{
    using Interfaces;
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class TodoItem : ISetEntity
    {
        public TodoItem()
        {
            Labels = new List<Label>();
        }

        public DateTime CreatedAt { get; set; }

        public DateTime? ClosedAt { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsDone { get; set; }

        public List<Label> Labels { get; set; }

        public string Id { get; set; }


        public class Label
        {
            public string Content { get; set; }

            public string ColorHex { get; set; }
        }
    }
}