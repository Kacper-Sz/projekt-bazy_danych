using App.Sessions;
using dll.Models;
using dll.Users;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.ViewModels
{
    class LoginViewModel : BaseViewModel
    {
        private string email;
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        private string password;
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        private string incorrectText;
        public string IncorrectText
        {
            get => incorrectText;
            set => SetProperty(ref incorrectText, value);
        }

        private bool incorrectIsVisible;
        public bool IncorrectIsVisible
        {
            get => incorrectIsVisible;
            set => SetProperty(ref incorrectIsVisible, value);
        }

        private RelayCommand loginCommand;
        public RelayCommand LoginCommand
        {
            get => loginCommand;
            set => SetProperty(ref loginCommand, value);
        }

        private RelayCommand registerCommand;
        public RelayCommand RegisterCommand
        {
            get => registerCommand;
            set => SetProperty(ref registerCommand, value);
        }

        public LoginViewModel()
        {
            IncorrectIsVisible = false;

            RegisterCommand = new RelayCommand(GoToRegisterPage);
            LoginCommand = new RelayCommand(Login);
        }

        private async Task Login(object? _)
        {
            try
            {
                IncorrectText = "Podano nie prawidłowe dane";
                UserManager userManager = new UserManager();
                User? user = await userManager.LoginAsync(Email, Password);
                if (user != null)
                {
                    IncorrectIsVisible = false;
                    UserSession.CurrentUser = user;
                    await Shell.Current.GoToAsync("//MainTabs");
                }
                else
                    IncorrectIsVisible = true;
            }
            catch(Exception ex)
            {
                IncorrectText = "Błąd aplikacji";
                IncorrectIsVisible = true;
            }
        }

        private async Task GoToRegisterPage(object? _)
            => await Shell.Current.GoToAsync("RegisterPage");
    }
}