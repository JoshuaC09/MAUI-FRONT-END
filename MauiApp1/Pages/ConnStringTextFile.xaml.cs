using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using MauiApp1.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using MySql.Data.MySqlClient;

namespace MauiApp1.Pages
{
    public partial class ConnStringTextFile : ContentPage
    {
        private string connectionString;

        private readonly HttpClientService _httpClientService;

        public ConnStringTextFile(HttpClientService httpClientService)
        {
            InitializeComponent();
            _httpClientService = httpClientService;
        }

        private async void OnSelectFileClicked(object sender, EventArgs e)
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Please select a BGC file",
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, new[] { "public.data" } }, // Update based on iOS MIME types if necessary
                    { DevicePlatform.Android, new[] { "application/octet-stream" } }, // Update based on Android MIME types if necessary
                    { DevicePlatform.WinUI, new[] { ".bgc" } },
                    { DevicePlatform.Tizen, new[] { "*/*" } }, // Generic MIME type to allow all files on Tizen
                })
            });

            if (result != null)
            {
                try
                {
                    var stream = await result.OpenReadAsync();
                    using (var reader = new StreamReader(stream))
                    {
                        var fileContent = await reader.ReadToEndAsync();
                        connectionString = fileContent.Trim();

                        if (!string.IsNullOrEmpty(connectionString))
                        {
                            StatusLabel.Text = "File uploaded successfully. Ready to validate.";
                            ValidateButton.IsEnabled = true;
                        }
                        else
                        {
                            StatusLabel.Text = "Failed to read connection string from the file.";
                            ValidateButton.IsEnabled = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    StatusLabel.Text = $"Error reading file: {ex.Message}";
                    ValidateButton.IsEnabled = false;
                }
            }
            else
            {
                StatusLabel.Text = "File selection canceled.";
                ValidateButton.IsEnabled = false;
            }
        }

        private async void OnValidateConnectionClicked(object sender, EventArgs e)
        {
            StatusLabel.Text = "Validating connection...";
            ValidateButton.IsEnabled = false;

            var validationResult = await Task.Run(() => ValidateConnectionString(connectionString));

            if (validationResult.StartsWith("Upload success"))
            {
                var apiResult = await _httpClientService.SetConnectionStringAsync(connectionString);
                if (apiResult)
                {
                    StatusLabel.Text = "Connection string validated and sent to API successfully.";
                }
                else
                {
                    StatusLabel.Text = "Validation successful, but failed to send connection string to API.";
                }
            }
            else
            {
                StatusLabel.Text = validationResult;
            }

            ValidateButton.IsEnabled = true;
        }

        private string ValidateConnectionString(string connStr)
        {
            try
            {
                using (var connection = new MySqlConnection(connStr))
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        return "Upload success. Connected to the database.";
                    }
                    else
                    {
                        return "Failed to connect to the database.";
                    }
                }
            }
            catch (MySqlException ex)
            {
                return $"MySQL error: {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}
