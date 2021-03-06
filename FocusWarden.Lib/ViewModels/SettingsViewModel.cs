using GalaSoft.MvvmLight;
using MediatR;

namespace FocusWarden.Lib.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        #region Fields
        private readonly IMediator _mediator;
        #endregion

        public SettingsViewModel(IMediator mediator)
        {
            this._mediator = mediator;
        }
    }
}
