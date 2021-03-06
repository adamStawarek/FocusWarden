using FocusWarden.Tests.UI.Reporting.Models;
using System.Threading.Tasks;

namespace FocusWarden.Tests.UI.Reporting.Interfaces
{
    public interface IReportHelper
    {
        void AddResult(TestResult testResult);

        void ClearResults();

        Task PublishAsync();
    }
}
