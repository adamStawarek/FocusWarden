using GalaSoft.MvvmLight;
using MediatR;

namespace FocusWarden.Lib.ViewModels
{
    public class PersonalizationViewModel : ViewModelBase
    {
        private readonly IMediator mediator;

        public PersonalizationViewModel(IMediator mediator)
        {
            this.mediator = mediator;
        }
    }
}
