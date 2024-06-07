using Microsoft.Maui.Controls;

namespace MauiApp1.Pages
{
    public partial class SettingsPage : ContentPage
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Port { get; set; }

        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = this;

            // Load the existing connection string components (if any)
            LoadConnectionSettings();
        }

        private void OnSaveClicked(object sender, EventArgs e)
        {
            // Save the connection string components
            SaveConnectionSettings();
        }

        private void LoadConnectionSettings()
        {
            // Example placeholder values; replace with logic to load from storage or configuration
            Server = "localhost";
            Database = "inventory_database";
            User = "root";
            Password = "BLUEGATES";
            Port = "3306";
        }

        private void SaveConnectionSettings()
        {
            // Example logic to save to storage or configuration
            string connectionString = $"Server={Server};Database={Database};User={User};Password={Password};Port={Port}";
            // Save the connection string to secure storage or configuration
        }
    }
}
