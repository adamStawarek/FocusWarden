namespace FocusWarden.Lib
{
    using DataAccess;
    using DataAccess.Interfaces;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Notifications;
    using System;
    using ViewModels;
    using ViewModels.Controls;

    public sealed class DependencyResolver
    {
        private static readonly Lazy<DependencyResolver> Lazy =
            new(() => new DependencyResolver());

        private DependencyResolver()
        {
        }

        public static DependencyResolver Instance => Lazy.Value;

        public IServiceProvider Services { get; private set; }

        public MainWindowViewModel Main => Services.GetService<MainWindowViewModel>();

        public SettingsViewModel Settings => Services.GetService<SettingsViewModel>();

        public SessionViewModel Session => Services.GetService<SessionViewModel>();

        public TaskFilterViewModel TasksFilter => Services.GetService<TaskFilterViewModel>();

        public StatisticsViewModel Statistics => Services.GetService<StatisticsViewModel>();

        public BacklogViewModel Backlog => Services.GetService<BacklogViewModel>();

        public void ConfigureServices()
        {
            Services = new ServiceCollection()
                .AddSingleton<IDataSettings, LocalSettings>()
                .AddMediatR(typeof(IDataSettings).Assembly)
                .AddSingleton<IToastNotification, ToastNotification>()
                .AddSingleton<TaskFilterViewModel>()
                .AddSingleton<MainWindowViewModel>()
                .AddSingleton<SettingsViewModel>()
                .AddSingleton<SessionViewModel>()
                .AddSingleton<StatisticsViewModel>()
                .AddSingleton<BacklogViewModel>()
                .BuildServiceProvider();
        }
    }
}