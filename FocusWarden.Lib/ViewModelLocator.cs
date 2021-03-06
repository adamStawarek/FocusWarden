using Autofac;
using FocusWarden.DataAccess;
using FocusWarden.DataAccess.Interfaces;
using FocusWarden.Lib.Notifications;
using FocusWarden.Lib.ViewModels;
using FocusWarden.Lib.ViewModels.Controls;
using MediatR.Extensions.Autofac.DependencyInjection;

namespace FocusWarden.Lib
{
    public class ViewModelLocator
    {
        private static IContainer container;

        static ViewModelLocator()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<LocalSettings>().As<IDataSettings>().SingleInstance();
            builder.RegisterMediatR((typeof(IDataSettings).Assembly));
            builder.RegisterType<ToastNotification>().As<IToastNotification>().SingleInstance();
            builder.RegisterType<TaskFilterViewModel>().SingleInstance();
            builder.RegisterType<MainWindowViewModel>().SingleInstance();
            builder.RegisterType<SettingsViewModel>().SingleInstance();
            builder.RegisterType<SessionViewModel>().SingleInstance();
            builder.RegisterType<StatisticsViewModel>().SingleInstance();
            builder.RegisterType<BacklogViewModel>().SingleInstance();

            container = builder.Build();
        }

        public MainWindowViewModel Main => container.Resolve<MainWindowViewModel>();

        public SettingsViewModel Settings => container.Resolve<SettingsViewModel>();

        public SessionViewModel Session => container.Resolve<SessionViewModel>();

        public TaskFilterViewModel TasksFilter => container.Resolve<TaskFilterViewModel>();

        public StatisticsViewModel Statistics => container.Resolve<StatisticsViewModel>();

        public BacklogViewModel Backlog => container.Resolve<BacklogViewModel>();
    }
}
