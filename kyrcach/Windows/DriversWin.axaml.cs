using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using kyrcach.Entities;
using MySqlConnector;

namespace kyrcach.Windows;

public partial class DriversWin : UserControl
{
    private Database _db = new Database();
    private ObservableCollection<Driver> _drivers = new ObservableCollection<Driver>();
    private string _sql = "select driverId, lastName, driverName, surName, photo, license from drivers";
    public DriversWin()
    {
        InitializeComponent();
        ShowDrivers();
    }
    public void ShowDrivers() {
        _db.OpenConnection();
        MySqlCommand command = new MySqlCommand(_sql, _db.GetConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows) {
            var curDriver = new Driver() {
                DriverId = reader.GetInt32("driverId"),
                LastName = reader.GetString("lastname"),
                DriverName = reader.GetString("driverName"),
                surNmae = reader.GetString("surName"),
                Photo = reader["photo"] as byte[],
                License = reader.GetString("license")
            };
            _drivers.Add(curDriver);
        }
        _db.CloseConnection();
        DriverLBox.ItemsSource = _drivers;
    }

    private void Back(object? sender, RoutedEventArgs e)
    {
       Panel.Children.Clear();
       Baza baza = new Baza();
       Panel.Children.Add(baza);
    }

    private void SeacrhDriver(object? sender, TextChangedEventArgs e)
    {
        var seacrhDriver = _drivers.Where(x =>
            x.DriverName.Contains(searchTb.Text, StringComparison.OrdinalIgnoreCase) ||
            x.License.Contains(searchTb.Text, StringComparison.OrdinalIgnoreCase)).ToList();
        DriverLBox.ItemsSource = seacrhDriver;
    }

    private void AddDriver(object? sender, RoutedEventArgs e)
    {
        Windows.AddDriver add = new AddDriver();
        Panel.Children.Clear();
        Panel.Children.Add(add);
    }
}