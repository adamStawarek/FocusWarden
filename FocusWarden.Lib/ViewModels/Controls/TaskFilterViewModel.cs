namespace FocusWarden.Lib.ViewModels.Controls
{
    using DataAccess.Domain.FilterSettings.Command;
    using DataAccess.Domain.FilterSettings.Query;
    using DataAccess.Models;
    using MediatR;
    using Microsoft.Toolkit.Mvvm.ComponentModel;
    using Microsoft.Toolkit.Mvvm.Input;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class TaskFilterViewModel : ObservableObject
    {
        #region Fields

        private readonly IMediator mediator;

        #endregion

        public TaskFilterViewModel(IMediator mediator)
        {
            this.mediator = mediator;

            FilterCommand = new AsyncRelayCommand(OnFilterSettingsChangedAsync);
            ResetCommand = new AsyncRelayCommand(ResetFilterSettingsAsync);

            GetFilterSettingsTask = this.mediator.Send(new GetFilterSettingsQuery());
        }

        private async Task OnFilterSettingsChangedAsync(CancellationToken cancellationToken)
        {
            await mediator.Send(new UpdateFilterSettingsCommand {Settings = FilterSettings}, cancellationToken);
            Filter?.Invoke(this, EventArgs.Empty);
            GetFilterSettingsTask = mediator.Send(new GetFilterSettingsQuery(), cancellationToken);
        }

        private async Task ResetFilterSettingsAsync(CancellationToken cancellationToken)
        {
            await mediator.Send(new UpdateFilterSettingsCommand {Settings = new FilterSettings()}, cancellationToken);
            Filter?.Invoke(this, EventArgs.Empty);
            GetFilterSettingsTask = mediator.Send(new GetFilterSettingsQuery(), cancellationToken);
        }

        #region Events & Properties

        public event EventHandler Filter;

        private TaskNotifier<FilterSettings> getFilterSettingsTask;

        private Task<FilterSettings> GetFilterSettingsTask
        {
            get => getFilterSettingsTask;
            set => SetPropertyAndNotifyOnCompletion(ref getFilterSettingsTask, value,
                _ => OnPropertyChanged(nameof(FilterSettings)));
        }

        public FilterSettings FilterSettings => GetFilterSettingsTask.Status == TaskStatus.RanToCompletion
            ? GetFilterSettingsTask.Result
            : null;

        #endregion

        #region Commands

        public IAsyncRelayCommand FilterCommand { get; }
        public IAsyncRelayCommand ResetCommand { get; }

        #endregion
    }
}