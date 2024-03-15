using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using kyrcach.Entities;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MySqlConnector;

namespace kyrcach.Windows;

public partial class AddDriver : UserControl
{
    public delegate void Closing();
    public event Closing onClosing;
    private Database _db;
    private Driver _driver;
    private byte[] _imageBytes;
    
    public AddDriver( )
    {
        InitializeComponent();
        _db = new Database();

    }

    private void BackBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        DriversWin drivers = new DriversWin();
        Panel.Children.Clear();
        Panel.Children.Add(drivers);
    }

    private void TinyVGashne(object? sender, RoutedEventArgs e)
    {
        try
        {   
            _db.OpenConnection();
            string sql = """
                            insert into drivers (lastName, driverName, surName, photo, license)
                            values (@last, @name, @sur, @photo, @liecense)
                         """;
            MySqlCommand command = new MySqlCommand(sql, _db.GetConnection());
            command.Parameters.Add("@last", DbType.String);
            command.Parameters["@last"].Value = LastNameTB.Text;
            command.Parameters.Add("@name", DbType.String);
            command.Parameters["@name"].Value = NameTB.Text;
            command.Parameters.Add("@sur", DbType.String);
            command.Parameters["@sur"].Value = SurNameTb.Text;
            command.Parameters.AddWithValue("@photo", _imageBytes);
            command.Parameters.Add("@liecense", DbType.String);
            command.Parameters["@liecense"].Value = LiecenseTB.Text;
            command.ExecuteNonQuery();
            _db.CloseConnection();
            onClosing.Invoke();
            Panel.Children.Clear();
            DriversWin drivers = new DriversWin();
            Panel.Children.Add(drivers);
        }
        catch (Exception exception)
        {
            MessageBox mes = new MessageBox();
            mes.ShowDialog(MainWindow.instance);
            Console.WriteLine("ERROR = " + exception);
        }
        
    }

    private async void DriverPhoto(object? sender, RoutedEventArgs e)
    {
        Bitmap bitmap;
        var topLevel = TopLevel.GetTopLevel(this);
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Выберите изображение",
            AllowMultiple = false
        });
        if (files.Count >= 1)
        {
            var imageStream = await files[0].OpenReadAsync();
            bitmap = new Bitmap(imageStream);
            PhotoBtn.Background = new ImageBrush(bitmap);
                //_imageBytes = await ConvertStreamToBytesAsync(imageStream); 
        }
    }
    private async Task<byte[]> ConvertStreamToBytesAsync(Stream stream)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            await stream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}