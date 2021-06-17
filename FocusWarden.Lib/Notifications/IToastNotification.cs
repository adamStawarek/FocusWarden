namespace FocusWarden.Lib.Notifications
{
    public interface IToastNotification
    {
        void ShowSuccess(string message);
        void ShowInfo(string message);
        void ShowWarning(string message);
        void ShowError(string message);
    }
}