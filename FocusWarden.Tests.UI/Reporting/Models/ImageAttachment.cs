using System.Windows.Media.Imaging;

namespace FocusWarden.Tests.UI.Reporting.Models
{
    public class ImageAttachment
    {
        public ImageAttachment(string name, BitmapImage bitmap)
        {
            Bitmap = bitmap;
            Name = name;
        }

        public BitmapImage Bitmap { get; }
        public string Name { get; }
    }
}
