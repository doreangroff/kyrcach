using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using kyrcach.Entities;
using MySqlConnector;

namespace kyrcach.Windows;

public partial class Baza : UserControl
{
    private Database _db = new Database();
    public Baza()
    {
        InitializeComponent();
        CountTables();
    }

    private void OpenDriversBaza(object? sender, TappedEventArgs e)
    {
        Panel.Children.Clear();
        DriversWin drive = new DriversWin();
        Panel.Children.Add(drive);
    }

    private void Back(object? sender, RoutedEventArgs e)
    {
        MainWindow main = new MainWindow();
        main.Show();
        
    }

    private void CountTables()
    {
        string drSql = "select COUNT(*) from drivers";
        string docSql = "select COUNT(*) from doctors";
        string cenSql = "select COUNT(*) from medCenters";
        string examSql = "select COUNT(*) from examinations";
        _db.OpenConnection();
        MySqlCommand command1 = new MySqlCommand(drSql, _db.GetConnection());
        MySqlCommand command2 = new MySqlCommand(docSql, _db.GetConnection());
        MySqlCommand command3 = new MySqlCommand(cenSql, _db.GetConnection());
        MySqlCommand command4 = new MySqlCommand(examSql, _db.GetConnection());
        driversCount.Text = command1.ExecuteScalar().ToString();
        doctorsCount.Text = command2.ExecuteScalar().ToString();
        centersCount.Text = command3.ExecuteScalar().ToString();
        examsCount.Text = command4.ExecuteScalar().ToString();

    }
}