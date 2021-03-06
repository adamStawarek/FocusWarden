using FocusWarden.Common;
using System.Windows;

namespace FocusWarden
{
    public partial class App : Application
    {
        public const string AppName = "FocusWarden";
        public const string Year = "2020";

        public App()
        {
            Product.AppName = AppName;
            Product.Year = Year;
        }
    }
}
