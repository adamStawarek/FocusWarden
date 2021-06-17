using FocusWarden.Common;
using FocusWarden.Lib;

namespace FocusWarden
{
    public partial class App
    {
        public const string AppName = "FocusWarden";
        public const string Year = "2020";

        public App()
        {
            Product.AppName = AppName;
            Product.Year = Year;
            
            DependencyResolver.Instance.ConfigureServices();
        }
    }
}
