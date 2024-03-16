using System;
using System.IO;
using Avalonia.Media.Imaging;

namespace kyrcach.Entities;

public class Driver
{
    public int DriverId { get; set; }
    public string LastName { get; set; }
    public string DriverName { get; set; }
    public string surNmae { get; set; }
    public byte[]? photo;
    
    public Bitmap Photo { get; set; }
    public string License { get; set; }
    public string Fullname => $"{LastName} {DriverName} {surNmae}";

    public void SetPhoto(byte[] but)
    {
        Console.WriteLine("BUTES = " + but.Length);
        if (but is byte[] bytes && but.Length > 0)
        {
            photo = but;
            using (var stream = new MemoryStream(bytes))
            {
                var bitmap = new Bitmap(stream);
                Photo = bitmap;
            }
        }
    }
}