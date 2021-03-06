using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Media.Imaging;

namespace FocusWarden.Tests.UI.Reporting.Models
{
    public class TestResult
    {
        public string TestSuite { get; set; }
        public string TestCase { get; set; }
        public int Priority { get; set; }
        public UnitTestOutcome Outcome { get; set; }
        public BitmapImage Attachment { get; set; }
        public string[] Exceptions { get; set; }
        public string[] Logs { get; set; }
        public string ScreenshootUrl { get; set; }
    }
}
