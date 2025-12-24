using dll;

namespace App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            ChangeConnectionString();

            MainPage = new AppShell();
        }

        private void ChangeConnectionString()
        {
            string ip = "192.168.1.73";
            DataManager.SetConnectionString($"mongodb://root:password@{ip}:1500/?authSource=admin");
        }
    }
}
