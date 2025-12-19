using CloudinaryDotNet.Actions;
using dll.Models;
using dll.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.ViewModels
{
    class RegisterViewModel : BaseViewModel
    {
        private string firstName;
        public string FirstName
        {
            get => firstName;
            set => SetProperty(ref firstName, value);
        }

        private string lastName;
        public string LastName
        {
            get => lastName;
            set => SetProperty(ref lastName, value);
        }

        private string email;
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        private string password1;
        public string Password1
        {
            get => password1;
            set => SetProperty(ref password1, value);
        }

        private string password2;
        public string Password2
        {
            get => password2;
            set => SetProperty(ref password2, value);
        }

        private string phoneNumber;
        public string PhoneNumber
        {
            get => phoneNumber;
            set => SetProperty(ref phoneNumber, value);
        }

        private string errorText;
        public string ErrorText
        {
            get => errorText;
            set => SetProperty(ref errorText, value);
        }

        private bool errorIsVisible;
        public bool ErrorIsVisible
        {
            get => errorIsVisible;
            set => SetProperty(ref errorIsVisible, value);
        }

        public RelayCommand RegisterCommand { get; set; }

        public RegisterViewModel()
        {
            SetInitValues();
        }

        private void SetInitValues()
        {
            ErrorIsVisible = false;

            RegisterCommand = new RelayCommand(Register);
        }

        private async Task Register(object? _)
        {
            ErrorText = "";
            ErrorIsVisible = false;
            try
            {
                if(Password1 != Password2)
                {
                    ErrorText = "Hasła nie są zgodne";
                    ErrorIsVisible = true;
                    return;
                }
                User newUser = CreateUserModel();
                UserManager userManager = new UserManager();
                UserRejestrationEnum result = await userManager.RegisterUserAsync(newUser);
                await ServeResult(result);
            }
            catch(Exception ex)
            {
                ErrorText = "Błąd aplikacji";
                ErrorIsVisible = true;
            }
        }

        private async Task ServeResult(UserRejestrationEnum result)
        {
            switch (result)
            {
                case UserRejestrationEnum.GOOD:
                    await Shell.Current.DisplayAlert("Sukces", "Konto zostało utworzone", "OK");
                    await Shell.Current.GoToAsync("..");
                    break;
                case UserRejestrationEnum.EMAIL_ALREADY_EXISTS:
                    ErrorText = "Email już istnieje";
                    ErrorIsVisible = true;
                    break;
                case UserRejestrationEnum.UNCOMPLETED_DATA:
                    ErrorText = "Nie wszystkie dane zostały wprowadzone";
                    ErrorIsVisible = true;
                    break;
                case UserRejestrationEnum.WEAK_PASSWORD:
                    ErrorText = "Hasło jest za słabe";
                    ErrorIsVisible = true;
                    break;
                case UserRejestrationEnum.INVALID_EMAIL_FORMAT:
                    ErrorText = "Niepoprawny format email";
                    ErrorIsVisible = true;
                    break;
                default:
                    ErrorText = "Nieznany błąd";
                    ErrorIsVisible = true;
                    break;
            }
        }

        private User CreateUserModel()
            => new User
            {
                FirstName = FirstName,
                LastName = FirstName,
                Email = Email,
                Password = Password1,
                Role = "customer",
                PhoneNumber = PhoneNumber,
                CreatedAt = DateTime.UtcNow,
            };
    }
}
