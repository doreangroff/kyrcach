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
    public void ShowDrivers()
    {
        _drivers.Clear();
        _db.OpenConnection();
        MySqlCommand command = new MySqlCommand(_sql, _db.GetConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows) {
            var curDriver = new Driver() {
                DriverId = reader.GetInt32("driverId"),
                LastName = reader.GetString("lastname"),
                DriverName = reader.GetString("driverName"),
                surNmae = reader.GetString("surName"),
                License = reader.GetString("license")
            };
            var ph = reader["photo"] as byte[];
            curDriver.SetPhoto(ph);
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
            x.LastName.Contains(searchTb.Text, StringComparison.OrdinalIgnoreCase) ||
            x.surNmae.Contains(searchTb.Text, StringComparison.OrdinalIgnoreCase) ||
            x.License.Contains(searchTb.Text, StringComparison.OrdinalIgnoreCase)).ToList();
        DriverLBox.ItemsSource = seacrhDriver;
    }

    private void AddDriver(object? sender, RoutedEventArgs e)
    {
        Windows.AddDriver add = new AddDriver();
        Panel.Children.Clear();
        Panel.Children.Add(add);
        add.onClosing += delegate { ShowDrivers(); };
    }

    private void DeleteDriver(object? sender, RoutedEventArgs e)
    {
        var selectedDriver = DriverLBox.SelectedItem as Driver;
        if (selectedDriver is null) return;

        DelWin del = new DelWin(selectedDriver);
        del.Closed += delegate { DriverLBox.ItemsSource = null; ShowDrivers(); };
        del.ShowDialog(MainWindow.instance);
    }

    private void EditDriver(object? sender, RoutedEventArgs e)
    {
        var selectedDriver = DriverLBox.SelectedItem as Driver;
        if (selectedDriver is null) return;

        EditDriver edit = new EditDriver(selectedDriver);
        Panel.Children.Clear();
        Panel.Children.Add(edit);
        edit.onClosing += delegate { ShowDrivers(); };
    }
}