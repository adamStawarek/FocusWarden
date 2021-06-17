namespace FocusWarden.Lib.ViewModels
{
    using MediatR;
    using Microsoft.Toolkit.Mvvm.ComponentModel;

    public class PersonalizationViewModel : ObservableObject
    {
        private readonly IMediator mediator;

        public PersonalizationViewModel(IMediator mediator)
        {
            this.mediator = mediator;
        }
    }
}