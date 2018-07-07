using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using CameraExample;
using CameraExample.iOS;

using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(SaveAndLoad))]
namespace CameraExample.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }
    }

	public class SaveAndLoad : IPhotoSL
	{
		public void SaveImage(Photo p)
		{
            //write the file to the local storage
			var directory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string jpgFilename = System.IO.Path.Combine(directory, p.FileName);
			File.WriteAllBytes(jpgFilename, p.FileData);

            //save that photo to the camera roll
			var mainImage = UIImage.FromFile(jpgFilename);
			mainImage.SaveToPhotosAlbum((image, error) =>{ });

		}
		public string LoadImage(int id)
		{
			var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, id.ToString() + ".jpg");
			return System.IO.File.ReadAllText(filePath);
		}
	}
}
