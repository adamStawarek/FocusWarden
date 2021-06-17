using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FocusWarden.UI.Controls
{
    public partial class TagListControl : IDisposable
    {
        public TagListControl()
        {
            InitializeComponent();
            WeakEventManager<TextBox, KeyEventArgs>.AddHandler(TagTextBox, nameof(KeyDown), OnKeyDown);
            WeakEventManager<TextBox, TextChangedEventArgs>.AddHandler(TagTextBox, nameof(TextBox.TextChanged), OnTextChanged);
        }
        
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox?.Text)) return;
            if (e.Key != Key.Enter && e.Key != Key.Tab) return;
            
            var labelBackgroundBrush = new SolidColorBrush
            {
                Color = Colors.Red
            };
            TagContainer.Children.Insert(0, new Label()
            {
                Content= textBox.Text, 
                Background = labelBackgroundBrush, 
                Margin = new Thickness(3)
            });
            textBox.Text = string.Empty;
            e.Handled = true;
        }
        
        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            SomeLabel.Visibility = sender is TextBox textBox && string.IsNullOrEmpty(textBox.Text) ? 
                Visibility.Visible : Visibility.Hidden;
        }

        public void Dispose()
        {
            WeakEventManager<TextBox, KeyEventArgs>.RemoveHandler(TagTextBox, nameof(TextBox.KeyDown), OnKeyDown);
            WeakEventManager<TextBox, TextChangedEventArgs>.RemoveHandler(TagTextBox, nameof(TextBox.TextChanged), OnTextChanged);
        }
    }
}
