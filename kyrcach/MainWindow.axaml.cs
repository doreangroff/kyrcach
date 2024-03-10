using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace kyrcach;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Close(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void ButtonOnFocus(object? sender, PointerEventArgs e)
    {
        var but = (Button)sender;
        string x = but.Name;
        Strelki(x, s1, s2, s3);
    }

    private void Strelki(string x, TextBlock s1, TextBlock s2, TextBlock s3)
    {
        switch (x)
        {
           case "Base":
               this.s1.Opacity = 1;
               this.s2.Opacity = 0;
               this.s3.Opacity = 0;
               break;
           case "Settings":
               this.s2.Opacity = 1;
               this.s1.Opacity = 0;
               this.s3.Opacity = 0;
               break;
           case "Exit":
               this.s3.Opacity = 1;
               this.s2.Opacity = 0;
               this.s1.Opacity = 0;
               break;
        }
    }
}