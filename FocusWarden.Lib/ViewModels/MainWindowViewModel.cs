using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows;

namespace FocusWarden.Lib.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public RelayCommand ExitAppCommand { get; set; }

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
