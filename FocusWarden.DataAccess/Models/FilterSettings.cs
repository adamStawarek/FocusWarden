using FocusWarden.Common.Enumerators;
using System;

namespace FocusWarden.DataAccess.Models
{
    [Serializable]
    public class FilterSettings
    {
        public FilterSetting<TodoItemStatus> Status { get; set; }

        public FilterSetting<DateTime> CreatedAt { get; set; }

        public FilterSetting<DateTime> ClosedAt { get; set; }

        public FilterSettings()
        {
            Status = new FilterSetting<TodoItemStatus>() { IsChecked = true, Value = TodoItemStatus.Open };
            CreatedAt = new FilterSetting<DateTime>() { IsChecked= true, Value = DateTime.Now };
            ClosedAt = new FilterSetting<DateTime>() { IsChecked = true, Value = DateTime.Now };
        }


        public class FilterSetting<T> where T : struct
        {
            public bool IsChecked { get; set; }
            public T Value { get; set; }
        }
    }
}
