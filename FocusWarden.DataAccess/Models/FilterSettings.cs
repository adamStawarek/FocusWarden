namespace FocusWarden.DataAccess.Models
{
    using Common.Enumerators;
    using System;

    [Serializable]
    public class FilterSettings
    {
        public FilterSettings()
        {
            Status = new FilterSetting<TodoItemStatus> {IsChecked = true, Value = TodoItemStatus.Open};
            CreatedAt = new FilterSetting<DateTime> {IsChecked = true, Value = DateTime.Now};
            ClosedAt = new FilterSetting<DateTime> {IsChecked = true, Value = DateTime.Now};
        }

        public FilterSetting<TodoItemStatus> Status { get; set; }

        public FilterSetting<DateTime> CreatedAt { get; set; }

        public FilterSetting<DateTime> ClosedAt { get; set; }


        public class FilterSetting<T> where T : struct
        {
            public bool IsChecked { get; set; }
            public T Value { get; set; }
        }
    }
}