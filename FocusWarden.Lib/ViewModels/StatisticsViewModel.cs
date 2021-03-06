using FocusWarden.DataAccess.Domain.FocusSessions.Query;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using MediatR;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FocusWarden.Lib.ViewModels
{
    public class StatisticsViewModel : ViewModelBase
    {
        #region Fields
        private readonly IMediator mediator;
        private string[] dailyLabels;
        private Func<double, string> dailyYFormatter;
        private string[] weeklyLabels;
        private Func<double, string> weeklyYFormatter;
        private string[] weekNumbers;
        #endregion

        #region Properties
        public SeriesCollection DailySeries { get; set; }
        public string[] DailyLabels
        {
            get => dailyLabels;
            set
            {
                if (dailyLabels == value) return;
                dailyLabels = value;
                RaisePropertyChanged();
            }
        }
        public Func<double, string> DailyYFormatter
        {
            get => dailyYFormatter;
            set
            {
                if (dailyYFormatter == value) return;
                dailyYFormatter = value;
                RaisePropertyChanged();
            }
        }

        public SeriesCollection WeeklySeries { get; set; }
        public string[] WeeklyLabels
        {
            get => weeklyLabels;
            set
            {
                if (weeklyLabels == value) return;
                weeklyLabels = value;
                RaisePropertyChanged();
            }
        }
        public Func<double, string> WeeklyYFormatter
        {
            get => weeklyYFormatter;
            set
            {
                if (weeklyYFormatter == value) return;
                weeklyYFormatter = value;
                RaisePropertyChanged();
            }
        }

        public ChartValues<HeatPoint> MonthlySeries { get; set; }
        public string[] WeekNumbers 
        { 
            get => weekNumbers;
            set
            {
                if (weekNumbers == value) return;
                weekNumbers = value;
                RaisePropertyChanged();
            }
        }
        public string[] WeekDays { get; set; }
        #endregion

        #region Commands
        public RelayCommand LoadedCommand { get; set; }
        #endregion

        public StatisticsViewModel(IMediator mediator)
        {
            this.mediator = mediator;

            DailySeries = new SeriesCollection();
            DailyYFormatter = value => value.ToString();
            DailyLabels = new string[0];

            WeeklySeries = new SeriesCollection();
            WeeklyYFormatter = value => value.ToString();
            WeeklyLabels = new string[0];

            MonthlySeries = new ChartValues<HeatPoint>();
            WeekNumbers = new string[0];
            var weekDays = new[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday };
            WeekDays = weekDays.Select(d => d.ToString()).ToArray();

            LoadedCommand = new RelayCommand(OnLoaded);
        }

        private async void OnLoaded()
        {
            var t1 = SetUpDailySessionCountChart();
            var t2 = SetUpWeeklySessionCountChart();
            var t3 = SetUpMonthlySessionCountChart();

            await Task.WhenAll(t1, t2, t3);
        }

        private async Task SetUpDailySessionCountChart()
        {
            var focusSession = await mediator.Send(new GetFocusSessionsQuery() { Date = DateTime.Now });

            var groupedByHour = focusSession.GroupBy(s => new { s.Date.Hour, s.IsCompleted })
                                            .ToDictionary(s => s.Key, s => s.Count());

            var completed = groupedByHour.Where(s => s.Key.IsCompleted).ToDictionary(s => s.Key.Hour, s => s.Value);
            var notCompleted = groupedByHour.Where(s => !s.Key.IsCompleted).ToDictionary(s => s.Key.Hour, s => s.Value);

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

            completed = completed.OrderBy(s => s.Key).ToDictionary(s => s.Key, s => s.Value);
            notCompleted = notCompleted.OrderBy(s => s.Key).ToDictionary(s => s.Key, s => s.Value);

            DailySeries.Clear();
            DailySeries.AddRange(new[]
            {
                new StackedColumnSeries
                {
                    Title = "Completed sessions",
                    Values = new ChartValues<int>(completed.Select(s=> s.Value)),
                    DataLabels = true
                },
                new StackedColumnSeries
                {
                    Title = "Partially completed sessions",
                    Values = new ChartValues<int>(notCompleted.Select(s=> s.Value)),
                    DataLabels = true
                }
            });

            DailyLabels = hours.Select(h => h.ToString()).ToArray();
        }

        private async Task SetUpWeeklySessionCountChart()
        {
            var currentDay = DateTime.Now.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)DateTime.Now.DayOfWeek;
            var fromDate = DateTime.Now.AddDays(1 - currentDay);
            var focusSession = await mediator.Send(new GetFocusSessionsQuery() { FromDate = fromDate, ToDate = DateTime.Now });

            var groupedByDay = focusSession.GroupBy(s => new { s.Date.DayOfWeek, s.IsCompleted })
                                            .ToDictionary(s => s.Key, s => s.Count());

            var completed = groupedByDay.Where(s => s.Key.IsCompleted).ToDictionary(s => s.Key.DayOfWeek, s => s.Value);
            var notCompleted = groupedByDay.Where(s => !s.Key.IsCompleted).ToDictionary(s => s.Key.DayOfWeek, s => s.Value);

            var days = Enumerable.Range(1, currentDay).ToList();
            days.ForEach(dayOfWeek =>
            {
                if (!completed.ContainsKey((DayOfWeek)((int)dayOfWeek % 7)))
                {
                    completed.Add((DayOfWeek)dayOfWeek, 0);
                }

                if (!notCompleted.ContainsKey((DayOfWeek)((int)dayOfWeek % 7)))
                {
                    notCompleted.Add((DayOfWeek)dayOfWeek, 0);
                }
            });

            completed = completed.OrderBy(s => s.Key.Equals(DayOfWeek.Sunday)).ThenBy(s => s.Key).ToDictionary(s => s.Key, s => s.Value);
            notCompleted = notCompleted.OrderBy(s => s.Key.Equals(DayOfWeek.Sunday)).ThenBy(s => s.Key).ToDictionary(s => s.Key, s => s.Value);

            WeeklySeries.Clear();
            WeeklySeries.AddRange(new[]
            {
                new StackedColumnSeries
                {
                    Title = "Completed sessions",
                    Values = new ChartValues<int>(completed.Select(s=> s.Value)),
                    DataLabels = true
                },
                new StackedColumnSeries
                {
                    Title = "Partially completed sessions",
                    Values = new ChartValues<int>(notCompleted.Select(s=> s.Value)),
                    DataLabels = true
                }
            });

            WeeklyLabels = days.Select(d => ((DayOfWeek)(d % 7)).ToString()).ToArray();
        }

        private async Task SetUpMonthlySessionCountChart()
        {
            var today = DateTime.Today;
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var focusSessions = await mediator.Send(new GetFocusSessionsQuery() { FromDate = firstDayOfMonth, ToDate = lastDayOfMonth, Completed = true });

            MonthlySeries.Clear();

            var currentWeekInMonth = 0;

            Enumerable.Range(1, DateTime.DaysInMonth(today.Year, today.Month)).ToList().ForEach(d =>
            {
                var day = firstDayOfMonth.AddDays(d - 1);

                var daySessions = focusSessions.Where(s => s.Date.Day == d).Count();

                var y = day.DayOfWeek == DayOfWeek.Sunday ? 6 : ((int)day.DayOfWeek) - 1;

                int x = currentWeekInMonth;

                MonthlySeries.Add(new HeatPoint() { X = x, Y = y, Weight = daySessions });

                if (day.DayOfWeek == DayOfWeek.Sunday) currentWeekInMonth++;
            });

            WeekNumbers = Enumerable.Range(0, currentWeekInMonth + 1).Select(s => s.ToString()).ToArray();
        }
    }
}
