using System;
using Xamarin.Forms;

namespace CameraExample
{
    public interface IPhotoSL
	{
		void SaveImage(Photo p);
		string LoadImage(int id);
	}
}
