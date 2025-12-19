using App.Views;
using dll;

namespace App
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            Routing.RegisterRoute(nameof(RegisterPage), typeof(Views.RegisterPage));
        }
    }
}
