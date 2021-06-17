using System;
using FocusWarden.DataAccess;
using FocusWarden.DataAccess.Interfaces;
using FocusWarden.Lib.Notifications;
using FocusWarden.Lib.ViewModels;
using FocusWarden.Lib.ViewModels.Controls;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FocusWarden.Lib
{
    public sealed class DependencyResolver
    {
        private DependencyResolver()
        {
        }

        private static readonly Lazy<DependencyResolver> Lazy =
            new(() => new DependencyResolver());

        public static DependencyResolver Instance => Lazy.Value;

        public IServiceProvider Services { get; private set; }

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

        public MainWindowViewModel Main => Services.GetService<MainWindowViewModel>();

        public SettingsViewModel Settings => Services.GetService<SettingsViewModel>();

        public SessionViewModel Session => Services.GetService<SessionViewModel>();

        public TaskFilterViewModel TasksFilter => Services.GetService<TaskFilterViewModel>();

        public StatisticsViewModel Statistics => Services.GetService<StatisticsViewModel>();

        public BacklogViewModel Backlog => Services.GetService<BacklogViewModel>();
    }
}