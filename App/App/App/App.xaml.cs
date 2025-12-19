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
            DataManager.SetConnectionString("mongodb://root:password@192.168.1.73:1500/?authSource=admin");
        }
    }
}
