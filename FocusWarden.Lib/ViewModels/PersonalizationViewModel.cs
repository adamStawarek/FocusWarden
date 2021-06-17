using MediatR;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace FocusWarden.Lib.ViewModels
{
    public class PersonalizationViewModel : ObservableObject
    {
        private readonly IMediator mediator;

        public PersonalizationViewModel(IMediator mediator)
        {
            this.mediator = mediator;
        }
    }
}
