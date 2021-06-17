namespace FocusWarden.Lib.ViewModels
{
    using Microsoft.Toolkit.Mvvm.ComponentModel;
    using Microsoft.Toolkit.Mvvm.Input;
    using System.Windows;

    public class MainWindowViewModel : ObservableObject
    {
        public MainWindowViewModel()
        {
            ExitAppCommand = new RelayCommand(ExitApp);
        }

        public IRelayCommand ExitAppCommand { get; }

        private void ExitApp()
        {
            Application.Current.Shutdown();
        }
    }
}