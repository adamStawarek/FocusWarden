using FocusWarden.DataAccess.Domain.FocusSessions.Query;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace FocusWarden.Lib.ViewModels
{
    public class StatisticsViewModel : ObservableObject
    {
        #region Fields

        private readonly IMediator mediator;
        private readonly Func<double, string> dailyYFormatter;
        private string[] dailyLabels;
        private string[] weeklyLabels;
        private Func<double, string> weeklyYFormatter;
        private string[] weekNumbers;

        #endregion

        #region Properties

        public SeriesCollection DailySeries { get; set; }

        public string[] DailyLabels
        {
            get => dailyLabels;
            set => SetProperty(ref dailyLabels, value);
        }

        public Func<double, string> DailyYFormatter
        {
            get => dailyYFormatter;
            init => SetProperty(ref dailyYFormatter, value);
        }

        public SeriesCollection WeeklySeries { get; set; }

        public string[] WeeklyLabels
        {
            get => weeklyLabels;
            set => SetProperty(ref weeklyLabels, value);
        }

        public Func<double, string> WeeklyYFormatter
        {
            get => weeklyYFormatter;
            set => SetProperty(ref weeklyYFormatter, value);
        }

        public ChartValues<HeatPoint> MonthlySeries { get; set; }

        public string[] WeekNumbers
        {
            get => weekNumbers;
            set => SetProperty(ref weekNumbers, value);
        }

        public string[] WeekDays { get; set; }

        #endregion

        #region Commands

        public IAsyncRelayCommand LoadedCommand { get; }

        #endregion

        public StatisticsViewModel(IMediator mediator)
        {
            this.mediator = mediator;

            DailySeries = new SeriesCollection();
            DailyYFormatter = value => value.ToString();
            DailyLabels = Array.Empty<string>();

            WeeklySeries = new SeriesCollection();
            WeeklyYFormatter = value => value.ToString();
            WeeklyLabels = Array.Empty<string>();

            MonthlySeries = new ChartValues<HeatPoint>();
            WeekNumbers = Array.Empty<string>();
            var weekDays = new[]
            {
                DayOfWeek.Monday,
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday,
                DayOfWeek.Friday,
                DayOfWeek.Saturday,
                DayOfWeek.Sunday
            };
            WeekDays = weekDays.Select(d => d.ToString()).ToArray();

            LoadedCommand = new AsyncRelayCommand(OnLoadedAsync);
        }

        private Task OnLoadedAsync()
        {
            var t1 = SetUpDailySessionCountChart();
            var t2 = SetUpWeeklySessionCountChart();
            var t3 = SetUpMonthlySessionCountChart();

            return Task.WhenAll(t1, t2, t3);
        }

        private async Task SetUpDailySessionCountChart()
        {
            var focusSession = await mediator.Send(
                new GetFocusSessionsQuery() {Date = DateTime.Now});

            var groupedByHour = focusSession
                .GroupBy(s => new {s.Date.Hour, s.IsCompleted})
                .ToDictionary(s => s.Key, s => s.Count());
            var completed = groupedByHour
                .Where(s => s.Key.IsCompleted)
                .ToDictionary(s => s.Key.Hour, s => s.Value);
            var notCompleted = groupedByHour
                .Where(s => !s.Key.IsCompleted)
                .ToDictionary(s => s.Key.Hour, s => s.Value);

            var hours = Enumerable.Range(0, DateTime.Now.Hour + 1).ToList();
            hours.ForEach(h =>
            {
                if (!completed.ContainsKey(h))
                {
                    completed.Add(h, 0);
                }

                if (!notCompleted.ContainsKey(h))
                {
                    notCompleted.Add(h, 0);
                }
            });

            completed = completed
                .OrderBy(s => s.Key)
                .ToDictionary(s => s.Key, s => s.Value);
            notCompleted = notCompleted
                .OrderBy(s => s.Key)
                .ToDictionary(s => s.Key, s => s.Value);

            DailySeries.Clear();
            DailySeries.AddRange(new[]
            {
                new StackedColumnSeries
                {
                    Title = "Completed sessions",
                    Values = new ChartValues<int>(completed.Select(s => s.Value)),
                    DataLabels = true
                },
                new StackedColumnSeries
                {
                    Title = "Partially completed sessions",
                    Values = new ChartValues<int>(notCompleted.Select(s => s.Value)),
                    DataLabels = true
                }
            });

            DailyLabels = hours.Select(h => h.ToString()).ToArray();
        }

        private async Task SetUpWeeklySessionCountChart()
        {
            var currentDay = DateTime.Now.DayOfWeek == DayOfWeek.Sunday ? 7 : (int) DateTime.Now.DayOfWeek;
            var fromDate = DateTime.Now.AddDays(1 - currentDay);
            var focusSession = await mediator.Send(
                new GetFocusSessionsQuery() {FromDate = fromDate, ToDate = DateTime.Now});

            var groupedByDay = focusSession
                .GroupBy(s => new {s.Date.DayOfWeek, s.IsCompleted})
                .ToDictionary(s => s.Key, s => s.Count());
            var completed = groupedByDay
                .Where(s => s.Key.IsCompleted)
                .ToDictionary(s => s.Key.DayOfWeek, s => s.Value);
            var notCompleted = groupedByDay
                .Where(s => !s.Key.IsCompleted)
                .ToDictionary(s => s.Key.DayOfWeek, s => s.Value);

            var days = Enumerable.Range(1, currentDay).ToList();
            days.ForEach(dayOfWeek =>
            {
                if (!completed.ContainsKey((DayOfWeek) ((int) dayOfWeek % 7)))
                {
                    completed.Add((DayOfWeek) dayOfWeek, 0);
                }

                if (!notCompleted.ContainsKey((DayOfWeek) ((int) dayOfWeek % 7)))
                {
                    notCompleted.Add((DayOfWeek) dayOfWeek, 0);
                }
            });

            completed = completed
                .OrderBy(s => s.Key.Equals(DayOfWeek.Sunday))
                .ThenBy(s => s.Key)
                .ToDictionary(s => s.Key, s => s.Value);
            notCompleted = notCompleted
                .OrderBy(s => s.Key.Equals(DayOfWeek.Sunday))
                .ThenBy(s => s.Key)
                .ToDictionary(s => s.Key, s => s.Value);

            WeeklySeries.Clear();
            WeeklySeries.AddRange(new[]
            {
                new StackedColumnSeries
                {
                    Title = "Completed sessions",
                    Values = new ChartValues<int>(completed.Select(s => s.Value)),
                    DataLabels = true
                },
                new StackedColumnSeries
                {
                    Title = "Partially completed sessions",
                    Values = new ChartValues<int>(notCompleted.Select(s => s.Value)),
                    DataLabels = true
                }
            });

            WeeklyLabels = days.Select(d => ((DayOfWeek) (d % 7)).ToString()).ToArray();
        }

        private async Task SetUpMonthlySessionCountChart()
        {
            var today = DateTime.Today;
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var focusSessions = await mediator.Send(
                new GetFocusSessionsQuery() {FromDate = firstDayOfMonth, ToDate = lastDayOfMonth, Completed = true});

            MonthlySeries.Clear();

            var currentWeekInMonth = 0;

            Enumerable.Range(1, DateTime.DaysInMonth(today.Year, today.Month)).ToList().ForEach(d =>
            {
                var day = firstDayOfMonth.AddDays(d - 1);

                var daySessions = focusSessions.Count(s => s.Date.Day == d);

                var y = day.DayOfWeek == DayOfWeek.Sunday ? 6 : ((int) day.DayOfWeek) - 1;

                var x = currentWeekInMonth;

                MonthlySeries.Add(new HeatPoint() {X = x, Y = y, Weight = daySessions});

                if (day.DayOfWeek == DayOfWeek.Sunday) currentWeekInMonth++;
            });

            WeekNumbers = Enumerable.Range(0, currentWeekInMonth + 1).Select(s => s.ToString()).ToArray();
        }
    }
}