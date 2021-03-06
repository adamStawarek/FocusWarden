using System.Configuration;
using System.Linq;

namespace FocusWarden.Tests.UI.Utils
{
    internal class Configuration
    {
        public static string ApplicationPath => ConfigurationManager.AppSettings["appPath"];
        public static string SendGridApiKey => ConfigurationManager.AppSettings["sendGridApiKey"];
        public static string SenderMail => ConfigurationManager.AppSettings["senderMail"];
        public static string ReceiverMail => ConfigurationManager.AppSettings["receiverMail"];
        public static string[] CCMailReceivers => ConfigurationManager.AppSettings["ccMailReceivers"]
                                                                        .Split(new[] { ';', ' ', ',' })
                                                                        .Where(a => !string.IsNullOrEmpty(a))
                                                                        .ToArray();
        public static string FtpHostName => ConfigurationManager.AppSettings["ftpHostName"];
        public static string FtpUser => ConfigurationManager.AppSettings["ftpUser"];
        public static string FtpPassword => ConfigurationManager.AppSettings["ftpPassword"];
        public static string FtpTargetDirectory => ConfigurationManager.AppSettings["ftpTargetDirectory"];
        public static int FtpPort => int.Parse(ConfigurationManager.AppSettings["ftpPort"]);
    }
}
