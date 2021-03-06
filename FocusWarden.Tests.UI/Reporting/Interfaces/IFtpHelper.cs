using System.Threading.Tasks;

namespace FocusWarden.Tests.UI.Reporting.Interfaces
{
    public interface IFtpHelper
    {
        Task<bool> TryCreateDirectoryAsync(string directory);
        Task<string[]> TryUploadFilesAsync(string[] files, string destDirectory);
    }
}
