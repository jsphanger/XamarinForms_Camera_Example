using System;
using System.IO;
using Xamarin.Forms;

namespace CameraExample
{
    public class Photo
    {
        public int ID { get; set; }
        public string FileName { get; set; }
        public int FileSize { get; set; }
        public byte[] FileData { get; set; }
        public string Location { get; set; }

        public Photo()
        {
            ID = -1;
            FileName = string.Empty;
            FileSize = 0;
            FileData = new byte[0];
            Location = string.Empty;
        }

        public ImageSource GetSource(){

            Stream s = new MemoryStream(FileData);

            return ImageSource.FromStream(() =>
            {
                return s;
            });

        }

    }
}
