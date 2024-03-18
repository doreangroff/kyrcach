using System;
using System.Data;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using kyrcach.Entities;
using MySqlConnector;

namespace kyrcach.Windows;

public partial class EditDriver : UserControl
{
    public delegate void Closing();
    public event Closing onClosing;
    private Database _db;
    private Driver _selecterDriver;
    private byte[] _imageBytes;
    
    public EditDriver(Driver selecterDriver)
    {
        InitializeComponent();
        _selecterDriver = selecterDriver;
        LastNameTB.Text = selecterDriver.LastName;
        NameTB.Text = selecterDriver.DriverName;
        SurNameTb.Text = selecterDriver.surNmae;
        LiecenseTB.Text = selecterDriver.License;
        _db = new Database();
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
            using var binaryReader = new BinaryReader(imageStream);
            Console.WriteLine("BIRIDER = " + binaryReader.BaseStream.Length);
            _imageBytes = ImageToByteArray(bitmap);
        }
    }
    
    public byte[] ImageToByteArray(Bitmap image)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            image.Save(stream);
            return(stream.ToArray());
        }
    }

    private void Edit(object? sender, RoutedEventArgs e)
    {
        try
        {   
            _db.OpenConnection();
            string sql = """
                            update drivers set lastName = @last,
                                               driverName = @name,
                                               surName = @name,
                                               photo = @photo, 
                                               license = @liecense
                            where driverId = @id
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
            command.Parameters.Add("@id", DbType.Int32);
            command.Parameters["@id"].Value = _selecterDriver.DriverId;
            command.ExecuteNonQuery();
            _db.CloseConnection();
            Panel.Children.Clear();
            DriversWin drivers = new DriversWin();
            Panel.Children.Add(drivers);
            onClosing.Invoke();
        }
        catch (Exception exception)
        {
            MessageBox mes = new MessageBox();
            mes.ShowDialog(MainWindow.instance);
            Console.WriteLine("ERROR = " + exception);
        }
    }

    private void BackBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        DriversWin drivers = new DriversWin();
        Panel.Children.Clear();
        Panel.Children.Add(drivers);
    }
}