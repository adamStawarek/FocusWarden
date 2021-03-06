using FocusWarden.DataAccess.Domain.FilterSettings.Command;
using FocusWarden.DataAccess.Domain.FilterSettings.Query;
using FocusWarden.DataAccess.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediatR;
using System;

namespace FocusWarden.Lib.ViewModels.Controls
{
    public class TaskFilterViewModel : ViewModelBase
    {
        #region Fields
        private IMediator _mediator;
        #endregion

        #region Events & Properties
        public event EventHandler Filter;

        public FilterSettings FilterSettings { get; set; } 
        #endregion

        #region Commands
        public RelayCommand FilterCommand { get; set; }

        public RelayCommand ResetCommand { get; set; } 
        #endregion

        public TaskFilterViewModel(IMediator mediator)
        {
            this._mediator = mediator;

            FilterSettings = _mediator.Send(new GetFilterSettingsQuery()).GetAwaiter().GetResult();

            FilterCommand = new RelayCommand(OnFilterChange);            
            ResetCommand = new RelayCommand(()=> { FilterSettings = new FilterSettings(); OnFilterChange(); });
        }

        private async void OnFilterChange()
        {
            await _mediator.Send(new UpdateFilterSettingsCommand() { Settings = FilterSettings });
            Filter?.Invoke(this, EventArgs.Empty);
            RaisePropertyChanged(nameof(FilterSettings));
        }
    }
}
