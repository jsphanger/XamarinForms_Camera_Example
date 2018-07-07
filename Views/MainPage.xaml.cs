using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Plugin.Media;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace CameraExample.Views
{
    public partial class MainPage : ContentPage
    {
        public List<Photo> photoCollection { get; set; }

        public MainPage()
        {
            InitializeComponent();

            photoCollection = new List<Photo>();
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
        }

        public async void CaptureButton_Clicked(object sender, EventArgs e)
        {

            //init the camera and check if we have access
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", "Could not access the camera. Please check your application settings and permissions.", "OK");
                return;
            }

            //Begin the take photo function and define some temp location for storage
            var fileid = Guid.NewGuid();
            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = fileid + ".jpg"
            });

            if (file == null)
                return;

            var stream = file.GetStream();
            byte[] data;

            using (BinaryReader br = new BinaryReader(stream))
            {
                data = br.ReadBytes((int)stream.Length);
            }

            Photo p = new Photo();

            p.ID = photoCollection.Count;
            p.FileData = data;
            p.FileSize = data.Length;
            p.FileName = fileid + ".jpg";
            p.Location = file.Path.Substring(0, file.Path.LastIndexOf('/') + 1);

            photoCollection.Add(p);
            PageImage.Source = p.GetSource();

            SaveButton.IsVisible = true;
            SaveButton.CommandParameter = p.ID;

        }

        public void SaveButton_Clicked(object sender, EventArgs e)
        {

            //grab a reference to our photo
            var btn = (Button)sender;

            //save the image to the file system
            DependencyService.Get<IPhotoSL>().SaveImage(photoCollection.Where(x => x.ID == Convert.ToInt32(btn.CommandParameter)).FirstOrDefault());

            //show success and update the UI
            StatusLabel.Opacity = 1;
            StatusLabel.Text = "Photo Saved to Library";
            StatusLabel.TextColor = Color.Green;
            StatusLabel.FontAttributes = FontAttributes.Bold;
            StatusLabel.FadeTo(0.0, 2500, null);

            PageImage.Source = null;

            SaveButton.IsVisible = false;

            //render list of roll items
            RenderItems();
        }

        public void DeleteButton_Clicked(object sender, EventArgs e)
        {

            var btn = (Button)sender;
            var id = Convert.ToInt32(btn.CommandParameter);

            //remove the image from the list
            photoCollection.Remove(photoCollection.Where(x => x.ID == id).FirstOrDefault());

            RenderItems();

            PageImage.Source = null;
            DeleteButton.IsVisible = false;
        }

        public void RenderItems()
        {

            PhotoRollList.Children.Clear();

            foreach (var item in photoCollection)
            {
				//ImageButton b = new ImageButton() { WidthRequest = 50.0, HeightRequest = 50.0 };
				//b.Text = item.ID.ToString();
				//b.Source = ImageSource.FromFile(Path.Combine(item.Location, item.FileName));

				//b.Clicked += (Object sender, EventArgs e) =>
				//{
				//PageImage.Source = item.GetSource();
				//DeleteButton.IsVisible = true;
				//DeleteButton.CommandParameter = item.ID;
				//};

                Image b = new Image() { Source = ImageSource.FromFile(Path.Combine(item.Location, item.FileName)), WidthRequest=50.0, HeightRequest=50.0 };
                b.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = new Command(() =>
                    {
                        PageImage.Source = item.GetSource();
                        DeleteButton.IsVisible = true;
                        DeleteButton.CommandParameter = item.ID;
                    }),
                    NumberOfTapsRequired = 1
                });


                PhotoRollList.Children.Add(b);
            }
        }
    }
}
