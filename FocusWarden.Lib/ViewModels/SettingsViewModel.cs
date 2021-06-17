namespace FocusWarden.Lib.ViewModels
{
    using MediatR;
    using Microsoft.Toolkit.Mvvm.ComponentModel;

    public class SettingsViewModel : ObservableObject
    {
        #region Fields

        private readonly IMediator mediator;

        #endregion

        public SettingsViewModel(IMediator mediator)
        {
            this.mediator = mediator;
        }
    }
}