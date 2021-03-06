using FocusWarden.Tests.UI.Reporting.Interfaces;
using FocusWarden.Tests.UI.Reporting.Models;
using FocusWarden.Tests.UI.Utils;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FocusWarden.Tests.UI.Reporting
{
    public sealed class EmailHelper : IEmailHelper
    {
        public async Task SendEmailAsync(string subject, string body, bool isBodyHtml = false, ImageAttachment[] screenshoots = null)
        {
            var client = new SendGridClient(Configuration.SendGridApiKey);
            var from = new EmailAddress(Configuration.SenderMail, "FocusWarden");
            var to = new EmailAddress(Configuration.ReceiverMail);
            var textContent = isBodyHtml ? string.Empty : body;
            var htmlContent = isBodyHtml ? body : string.Empty;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, textContent, htmlContent);

            if (Configuration.CCMailReceivers.Any())
                msg.AddCcs(Configuration.CCMailReceivers.Select(a => new EmailAddress(a)).ToList());

            if (screenshoots != null)
            {
                AttachScreenshoots(screenshoots, msg);
            }

            try
            {
                await client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void AttachScreenshoots(ImageAttachment[] screenshoots, SendGridMessage msg)
        {
            foreach (var screenshot in screenshoots)
            {
                if (screenshot == null) continue;
                msg.AddAttachment(screenshot.Name, Convert.ToBase64String(BufferFromImage(screenshot.Bitmap)));
            }
        }

        private byte[] BufferFromImage(BitmapImage bitmapImage)
        {
            MemoryStream stream = (MemoryStream)bitmapImage.StreamSource;
            return stream.ToArray();
        }
    }
}
