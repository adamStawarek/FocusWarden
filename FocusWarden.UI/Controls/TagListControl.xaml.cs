using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FocusWarden.UI.Controls
{
    public partial class TagListControl : UserControl
    {
        public TagListControl()
        {
            InitializeComponent();
        }

        private void TextBlock_KeyDown(object sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox?.Text)) return;

            if (e.Key == Key.Enter || e.Key == Key.Tab)
            {             
                var labelBackgroundBrush = new SolidColorBrush();
                labelBackgroundBrush.Color = Colors.Red;
                TagContainer.Children.Insert(0, new Label() { Content= textBox.Text, Background = labelBackgroundBrush, Margin = new System.Windows.Thickness(3) });
                textBox.Text = string.Empty;
                e.Handled = true;
            }
        }

        private void TagTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            SomeLabel.Visibility = string.IsNullOrEmpty(textBox.Text) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
        }
    }
}
