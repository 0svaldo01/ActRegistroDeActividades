using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using System;
using System.IO;
using System.Net.Http;


namespace U3ActRegistroDeActividadesMaui.Helpers
{
    public class ImagenABase64
    {
        private string base64Image;
        private async void OnSelectImageClicked(object sender, EventArgs e)
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Select an image",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    using (var stream = await result.OpenReadAsync())
                    {
                        var memoryStream = new MemoryStream();
                        await stream.CopyToAsync(memoryStream);
                        byte[] imageBytes = memoryStream.ToArray();
                        base64Image = Convert.ToBase64String(imageBytes);

                        SelectedImage.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                        ((Button)FindByName("SendImage")).IsEnabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void OnSendImageClicked(object sender, EventArgs e)
        {
            string apiUrl = "https://your-api-url.com/upload"; // Reemplaza con la URL de tu API

            await SendImageToApi(base64Image, apiUrl);
        }

        private async Task SendImageToApi(string base64Image, string apiUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                var content = new StringContent($"{{\"image\":\"{base64Image}\"}}", Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("Success", "Image uploaded successfully!", "OK");
                }
                else
                {
                    await DisplayAlert("Error", $"Failed to upload image. Status code: {response.StatusCode}", "OK");
                }
            }
        } 
    }
}
