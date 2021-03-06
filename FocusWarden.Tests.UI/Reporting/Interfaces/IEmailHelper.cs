using FocusWarden.Tests.UI.Reporting.Models;
using System.Threading.Tasks;

namespace FocusWarden.Tests.UI.Reporting.Interfaces
{
    public interface IEmailHelper
    {
        Task SendEmailAsync(string subject, string body, bool isBodyHtml = false, ImageAttachment[] screenshoots = null);
    }
}
