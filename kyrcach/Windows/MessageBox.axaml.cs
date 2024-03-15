using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace kyrcach.Windows;

public partial class MessageBox : Window
{
    public MessageBox()
    {
        InitializeComponent();
    }

    private void Ok(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
}