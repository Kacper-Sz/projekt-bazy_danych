using dll;
using dll.Models;
using dll.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class UserManagerTests
    {
        private readonly UserManager userManager;

        public UserManagerTests()
        {
            MongoDbManager dbManager = new MongoDbManager(DataManager.ConnectionString(), DataManager.DatabaseName());
            userManager = new UserManager(dbManager.Database);
        }

        #region TestLoginAsync
        #region IncorrectData
        [Fact]
        public async Task LoginAsyncTest1()
        {
            User? user = await userManager.LoginAsync("incorrect@email.com", "incorrectPassword12345");
            Assert.Null(user);
        }

        [Fact]
        public async Task LoginAsyncTest2()
        {
            User? user = await userManager.LoginAsync("jan.kowalski@example.com", "incorrectPassword12345");
            Assert.Null(user);
        }

        [Fact]
        public async Task LoginAsyncTest3()
        {
            User? user = await userManager.LoginAsync("incorrect@email.com", "hashed_password_123");
            Assert.Null(user);
        }
        #endregion

        #region CorrectData
        [Fact]
        public async Task LoginAsyncTest4()
        {
            User? user = await userManager.LoginAsync("jan.kowalski@example.com", "hashed_password_123");
            Assert.NotNull(user);
        }
        #endregion
        #endregion

        #region TestRegisterUserAsync
        #region UncompletedData
        [Fact]
        public async Task RegisterUserAsyncTest1()
        {
            User user = new User
            {
                FirstName = "",
                LastName = "Kwiatkowski",
                Email = "Roman.Kwiatkowski@example.com",
                Role = "Customer",
                Password = "Password12345",
                PhoneNumber = "+48123456789",
                CreatedAt = DateTime.UtcNow,
            };
            var result = await userManager.RegisterUserAsync(user);
            Assert.Equal(UserRejestrationEnum.UNCOMPLETED_DATA, result);
        }

        [Fact]
        public async Task RegisterUserAsyncTest2()
        {
            User user = new User
            {
                FirstName = "Roman",
                LastName = "",
                Email = "Roman.Kwiatkowski@example.com",
                Role = "Customer",
                Password = "Password12345",
                PhoneNumber = "+48123456789",
                CreatedAt = DateTime.UtcNow,
            };
            var result = await userManager.RegisterUserAsync(user);
            Assert.Equal(UserRejestrationEnum.UNCOMPLETED_DATA, result);
        }

        [Fact]
        public async Task RegisterUserAsyncTest3()
        {
            User user = new User
            {
                FirstName = "Roman",
                LastName = "Kwiatkowski",
                Email = "",
                Role = "Customer",
                Password = "Password12345",
                PhoneNumber = "+48123456789",
                CreatedAt = DateTime.UtcNow,
            };
            var result = await userManager.RegisterUserAsync(user);
            Assert.Equal(UserRejestrationEnum.UNCOMPLETED_DATA, result);
        }

        [Fact]
        public async Task RegisterUserAsyncTest4()
        {
            User user = new User
            {
                FirstName = "Roman",
                LastName = "Kwiatkowski",
                Email = "Roman.Kwiatkowski@example.com",
                Role = "",
                Password = "Password12345",
                PhoneNumber = "+48123456789",
                CreatedAt = DateTime.UtcNow,
            };
            var result = await userManager.RegisterUserAsync(user);
            Assert.Equal(UserRejestrationEnum.UNCOMPLETED_DATA, result);
        }

        [Fact]
        public async Task RegisterUserAsyncTest5()
        {
            User user = new User
            {
                FirstName = "Roman",
                LastName = "Kwiatkowski",
                Email = "Roman.Kwiatkowski@example.com",
                Role = "Customer",
                Password = "",
                PhoneNumber = "+48123456789",
                CreatedAt = DateTime.UtcNow,
            };
            var result = await userManager.RegisterUserAsync(user);
            Assert.Equal(UserRejestrationEnum.UNCOMPLETED_DATA, result);
        }

        [Fact]
        public async Task RegisterUserAsyncTest6()
        {
            User user = new User
            {
                FirstName = "Roman",
                LastName = "Kwiatkowski",
                Email = "Roman.Kwiatkowski@example.com",
                Role = "Customer",
                Password = "Password12345",
                PhoneNumber = "",
                CreatedAt = DateTime.UtcNow,
            };
            var result = await userManager.RegisterUserAsync(user);
            Assert.Equal(UserRejestrationEnum.UNCOMPLETED_DATA, result);
        }

        [Fact]
        public async Task RegisterUserAsyncTest7()
        {
            User user = new User
            {
                FirstName = "Roman",
                LastName = "Kwiatkowski",
                Email = "Roman.Kwiatkowski@example.com",
                Role = "Customer",
                Password = "Password12345",
                PhoneNumber = "+48123456789",
            };
            var result = await userManager.RegisterUserAsync(user);
            Assert.Equal(UserRejestrationEnum.UNCOMPLETED_DATA, result);
        }
        #endregion

        #region EmailAlreadyExists
        [Fact]
        public async Task RegisterUserAsyncTest8()
        {
            User user = new User
            {
                FirstName = "Roman",
                LastName = "Kwiatkowski",
                Email = "jan.kowalski@example.com",
                Role = "Customer",
                Password = "Password12345",
                PhoneNumber = "+48123456789",
                CreatedAt = DateTime.UtcNow,
            };
            var result = await userManager.RegisterUserAsync(user);
            Assert.Equal(UserRejestrationEnum.EMAIL_ALREADY_EXISTS, result);
        }
        #endregion

        #region WeakPassword
        [Fact]
        public async Task RegisterUserAsyncTest9()
        {
            User user = new User
            {
                FirstName = "Roman",
                LastName = "Kwiatkowski",
                Email = "Roman.Kwiatkowski@example.com",
                Role = "Customer",
                Password = "Pa123",
                PhoneNumber = "+48123456789",
                CreatedAt = DateTime.UtcNow,
            };
            var result = await userManager.RegisterUserAsync(user);
            Assert.Equal(UserRejestrationEnum.WEAK_PASSWORD, result);
        }

        [Fact]
        public async Task RegisterUserAsyncTest10()
        {
            User user = new User
            {
                FirstName = "Roman",
                LastName = "Kwiatkowski",
                Email = "Roman.Kwiatkowski@example.com",
                Role = "Customer",
                Password = "password12345",
                PhoneNumber = "+48123456789",
                CreatedAt = DateTime.UtcNow,
            };
            var result = await userManager.RegisterUserAsync(user);
            Assert.Equal(UserRejestrationEnum.WEAK_PASSWORD, result);
        }

        [Fact]
        public async Task RegisterUserAsyncTest11()
        {
            User user = new User
            {
                FirstName = "Roman",
                LastName = "Kwiatkowski",
                Email = "Roman.Kwiatkowski@example.com",
                Role = "Customer",
                Password = "PASSWORD12345",
                PhoneNumber = "+48123456789",
                CreatedAt = DateTime.UtcNow,
            };
            var result = await userManager.RegisterUserAsync(user);
            Assert.Equal(UserRejestrationEnum.WEAK_PASSWORD, result);
        }

        [Fact]
        public async Task RegisterUserAsyncTest12()
        {
            User user = new User
            {
                FirstName = "Roman",
                LastName = "Kwiatkowski",
                Email = "Roman.Kwiatkowski@example.com",
                Role = "Customer",
                Password = "Password",
                PhoneNumber = "+48123456789",
                CreatedAt = DateTime.UtcNow,
            };
            var result = await userManager.RegisterUserAsync(user);
            Assert.Equal(UserRejestrationEnum.WEAK_PASSWORD, result);
        }

        [Fact]
        public async Task RegisterUserAsyncTest13()
        {
            User user = new User
            {
                FirstName = "Roman",
                LastName = "Kwiatkowski",
                Email = "Roman.Kwiatkowski@example.com",
                Role = "Customer",
                Password = "1234567890",
                PhoneNumber = "+48123456789",
                CreatedAt = DateTime.UtcNow,
            };
            var result = await userManager.RegisterUserAsync(user);
            Assert.Equal(UserRejestrationEnum.WEAK_PASSWORD, result);
        }
        #endregion

        #region InvalidEmailFormat
        [Fact]
        public async Task RegisterUserAsyncTest14()
        {
            User user = new User
            {
                FirstName = "Roman",
                LastName = "Kwiatkowski",
                Email = "Roman.Kwiatkowski@example",
                Role = "Customer",
                Password = "$Password12345",
                PhoneNumber = "+48123456789",
                CreatedAt = DateTime.UtcNow,
            };
            var result = await userManager.RegisterUserAsync(user);
            Assert.Equal(UserRejestrationEnum.INVALID_EMAIL_FORMAT, result);
        }

        [Fact]
        public async Task RegisterUserAsyncTest15()
        {
            User user = new User
            {
                FirstName = "Roman",
                LastName = "Kwiatkowski",
                Email = "Roman.Kwiatkowski@example.c",
                Role = "Customer",
                Password = "$Password12345",
                PhoneNumber = "+48123456789",
                CreatedAt = DateTime.UtcNow,
            };
            var result = await userManager.RegisterUserAsync(user);
            Assert.Equal(UserRejestrationEnum.INVALID_EMAIL_FORMAT, result);
        }

        [Fact]
        public async Task RegisterUserAsyncTest16()
        {
            User user = new User
            {
                FirstName = "Roman",
                LastName = "Kwiatkowski",
                Email = "Roman.Kwiatkowski@example.comcomcom",
                Role = "Customer",
                Password = "$Password12345",
                PhoneNumber = "+48123456789",
                CreatedAt = DateTime.UtcNow,
            };
            var result = await userManager.RegisterUserAsync(user);
            Assert.Equal(UserRejestrationEnum.INVALID_EMAIL_FORMAT, result);
        }

        [Fact]
        public async Task RegisterUserAsyncTest17()
        {
            User user = new User
            {
                FirstName = "Roman",
                LastName = "Kwiatkowski",
                Email = "Roman.Kwiatkowskiexample.com",
                Role = "Customer",
                Password = "$Password12345",
                PhoneNumber = "+48123456789",
                CreatedAt = DateTime.UtcNow,
            };
            var result = await userManager.RegisterUserAsync(user);
            Assert.Equal(UserRejestrationEnum.INVALID_EMAIL_FORMAT, result);
        }

        [Fact]
        public async Task RegisterUserAsyncTest18()
        {
            User user = new User
            {
                FirstName = "Roman",
                LastName = "Kwiatkowski",
                Email = "@example.com",
                Role = "Customer",
                Password = "$Password12345",
                PhoneNumber = "+48123456789",
                CreatedAt = DateTime.UtcNow,
            };
            var result = await userManager.RegisterUserAsync(user);
            Assert.Equal(UserRejestrationEnum.INVALID_EMAIL_FORMAT, result);
        }

        [Fact]
        public async Task RegisterUserAsyncTest19()
        {
            User user = new User
            {
                FirstName = "Roman",
                LastName = "Kwiatkowski",
                Email = "Roman.Kwiatkowski@@example.com",
                Role = "Customer",
                Password = "$Password12345",
                PhoneNumber = "+48123456789",
                CreatedAt = DateTime.UtcNow,
            };
            var result = await userManager.RegisterUserAsync(user);
            Assert.Equal(UserRejestrationEnum.INVALID_EMAIL_FORMAT, result);
        }

        [Fact]
        public async Task RegisterUserAsyncTest20()
        {
            User user = new User
            {
                FirstName = "Roman",
                LastName = "Kwiatkowski",
                Email = "Roman.Kwiatkowski@example..com",
                Role = "Customer",
                Password = "$Password12345",
                PhoneNumber = "+48123456789",
                CreatedAt = DateTime.UtcNow,
            };
            var result = await userManager.RegisterUserAsync(user);
            Assert.Equal(UserRejestrationEnum.INVALID_EMAIL_FORMAT, result);
        }
        #endregion

        #region Good
        [Fact]
        public async Task RegisterUserAsyncTest21()
        {
            User user = new User
            {
                FirstName = "Roman",
                LastName = "Kwiatkowski",
                Email = "kwiatkowski.roman@example.com",
                Role = "Customer",
                Password = "$Password12345",
                PhoneNumber = "+48123456789",
                CreatedAt = DateTime.UtcNow,
            };
            var result = await userManager.RegisterUserAsync(user);
            Assert.Equal(UserRejestrationEnum.GOOD, result);
        }
        #endregion
        #endregion
    }
}
