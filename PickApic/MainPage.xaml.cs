using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PickApic
{
    public partial class MainPage : ContentPage
    {
        public string PhotoPath { get; private set; }

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

         async void TakePhotoAsync(object sender, EventArgs e)
        {
            try
            {
                FileResult photo = await MediaPicker.CapturePhotoAsync();
                await LoadPhotoAsync(photo);
                Console.WriteLine($"CapturePhotoAsync COMPLETED: {PhotoPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
        }

         async void PickPhotoAsync(object sender, EventArgs e)
        {
            try
            {
                FileResult photo = await MediaPicker.PickPhotoAsync();
                await LoadPhotoAsync(photo);
                Console.WriteLine($"CapturePhotoAsync COMPLETED: {PhotoPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
        }

        private async Task LoadPhotoAsync(FileResult photo)
        {
            // canceled
            if (photo == null)
            {
                PhotoPath = null;
                return;
            }
            // save the file into local storage
            string newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using (var stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
                await stream.CopyToAsync(newStream);
            Image.Source = ImageSource.FromFile(newFile);
            PhotoPath = newFile;
        }

        async void SharePhotoAsync(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(PhotoPath)) return;
            
            await Share.RequestAsync(new ShareFileRequest
            {
                Title = Title,
                File = new ShareFile(PhotoPath)
            });
        }
    }
}
