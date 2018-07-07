using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Xamarin.Forms;
using CameraExample.Droid;
using System.IO;

[assembly: Dependency(typeof(SaveAndLoad))]
namespace CameraExample.Droid
{
    [Activity(Label = "CameraExample.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App());
        }
    }

	public class SaveAndLoad : IPhotoSL
	{
		public void SaveImage(Photo p)
		{
            //write the file to the local storage
            var directory = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
            if(!Directory.Exists(directory.Path)){
                Directory.CreateDirectory(directory.Path);
            }

            //save image to camera roll
			string jpgFilename = System.IO.Path.Combine(directory.Path, p.FileName);
			File.WriteAllBytes(jpgFilename, p.FileData);
		}
		public string LoadImage(int id)
		{
			var documentsPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures).Path;
			var filePath = Path.Combine(documentsPath, id.ToString() + ".jpg");
			return System.IO.File.ReadAllText(filePath);
		}
	}
}
