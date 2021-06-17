using System.Windows;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace FocusWarden.Lib.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        public IRelayCommand ExitAppCommand { get; }

        public MainWindowViewModel()
        {
            ExitAppCommand = new RelayCommand(ExitApp);
        }

        private void ExitApp()
        {
            Application.Current.Shutdown();
        }
    }
}