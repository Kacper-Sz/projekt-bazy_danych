using dll.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace dll.Users
{
    public class UserManager
    {
        private const string COLLECTION_NAME = "users";
        private const int MIN_PASSWORD_LENGTH = 8;

        private readonly IMongoCollection<User> _users;

        public UserManager()
        {
            MongoDbManager mongoDbManager = new MongoDbManager();
            _users = mongoDbManager.Database.GetCollection<User>(COLLECTION_NAME);
        }

        public async Task<User?> LoginAsync(string email, string password)
            => await _users.Find(u => u.Email == email && u.Password == password).FirstOrDefaultAsync();

        public async Task<UserRejestrationEnum> RegisterUserAsync(User user)
        {
            if (!ValidateUserData(user))
                return UserRejestrationEnum.UNCOMPLETED_DATA;
            User existingUser = await _users.Find(u => u.Email == user.Email).FirstOrDefaultAsync();
            if (existingUser != null)
                return UserRejestrationEnum.EMAIL_ALREADY_EXISTS;
            
            var passwordValidationResult = ValidatePasswordDetailed(user.Password);
            if (passwordValidationResult != UserRejestrationEnum.GOOD)
                return passwordValidationResult;
            
            if (!ValidateEmailFormat(user.Email))
                return UserRejestrationEnum.INVALID_EMAIL_FORMAT;
            await _users.InsertOneAsync(user);
            return UserRejestrationEnum.GOOD;
        }

        private bool ValidateUserData(User user)
            => !string.IsNullOrWhiteSpace(user.FirstName)
                && !string.IsNullOrWhiteSpace(user.LastName)
                && !string.IsNullOrWhiteSpace(user.Email)
                && !string.IsNullOrWhiteSpace(user.Password)
                && !string.IsNullOrWhiteSpace(user.Role)
                && !string.IsNullOrWhiteSpace(user.PhoneNumber)
                && user.CreatedAt != default;

        private bool ValidatePassword(string password)
            => password.Length >= MIN_PASSWORD_LENGTH 
                && password.Any(char.IsUpper) 
                && password.Any(char.IsLower) 
                && password.Any(char.IsDigit) 
                && password.Any(ch => !char.IsLetterOrDigit(ch));

        private UserRejestrationEnum ValidatePasswordDetailed(string password)
        {
            if (password.Length < MIN_PASSWORD_LENGTH)
                return UserRejestrationEnum.PASSWORD_TOO_SHORT;
            
            if (!password.Any(char.IsUpper))
                return UserRejestrationEnum.PASSWORD_MISSING_UPPERCASE;
            
            if (!password.Any(char.IsLower))
                return UserRejestrationEnum.PASSWORD_MISSING_LOWERCASE;
            
            if (!password.Any(char.IsDigit))
                return UserRejestrationEnum.PASSWORD_MISSING_DIGIT;
            
            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
                return UserRejestrationEnum.PASSWORD_MISSING_SPECIAL_CHAR;
            
            return UserRejestrationEnum.GOOD;
        }

        private bool ValidateEmailFormat(string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (!match.Success)
                return false;
            return true;
        }
    }
}
