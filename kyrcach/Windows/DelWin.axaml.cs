using System;
using System.Data;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using kyrcach.Entities;
using MySqlConnector;

namespace kyrcach.Windows;


public partial class DelWin : Window
{
    private Database _db;
    private readonly Driver _selectedDriver;
    public DelWin(Driver selectedDriver)
    {
        
        InitializeComponent();
        _selectedDriver = selectedDriver;
        _db = new Database();
    }

    private void Da(object? sender, RoutedEventArgs e)
    {
        _db.OpenConnection();
        string sql = """
                        SET FOREIGN_KEY_CHECKS=0;
                     Delete from drivers where driverId = @id LIMIT 1;
                     """;
        MySqlCommand command = new MySqlCommand(sql, _db.GetConnection());
        command.Parameters.Add("@id", DbType.Int32);
        command.Parameters["@id"].Value = _selectedDriver.DriverId;
        command.ExecuteNonQuery();
        _db.CloseConnection();
        this.Close();
    }

    private void Net(object? sender, RoutedEventArgs e)
    { 
        this.Close();
    }
}