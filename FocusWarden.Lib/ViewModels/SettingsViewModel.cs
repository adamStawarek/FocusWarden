using MediatR;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace FocusWarden.Lib.ViewModels
{
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
