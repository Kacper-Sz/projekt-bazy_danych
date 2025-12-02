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
        const string connectionString = "mongodb://root:password@localhost:1500/?authSource=admin";
        const string databaseName = "shop";

        private readonly UserManager userManager;

        public UserManagerTests()
        {
            MongoDbManager dbManager = new MongoDbManager(connectionString, databaseName);
            userManager = new UserManager(dbManager.Database);
        }

        #region TestLoginAsync
        #region IncorrectData
        [Fact]
        public async Task Test1LoginAsync()
        {
            User? user = await userManager.LoginAsync("incorrect@email.com", "incorrectPassword12345");
            Assert.Null(user);
        }

        [Fact]
        public async Task Test2LoginAsync()
        {
            User? user = await userManager.LoginAsync("jan.kowalski@example.com", "incorrectPassword12345");
            Assert.Null(user);
        }

        [Fact]
        public async Task Test3LoginAsync()
        {
            User? user = await userManager.LoginAsync("incorrect@email.com", "hashed_password_123");
            Assert.Null(user);
        }
        #endregion

        #region CorrectData
        [Fact]
        public async Task Test4LoginAsync()
        {
            User? user = await userManager.LoginAsync("jan.kowalski@example.com", "hashed_password_123");
            Assert.NotNull(user);
        }
        #endregion
        #endregion

        #region TestRegisterUserAsync
        #region UncompletedData
        [Fact]
        public async Task Test1RegisterUserAsync()
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
        public async Task Test2RegisterUserAsync()
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
        public async Task Test3RegisterUserAsync()
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
        public async Task Test4RegisterUserAsync()
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
        public async Task Test5RegisterUserAsync()
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
        public async Task Test6RegisterUserAsync()
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
        public async Task Test7RegisterUserAsync()
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
        public async Task Test8RegisterUserAsync()
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
        public async Task Test9RegisterUserAsync()
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
        public async Task Test10RegisterUserAsync()
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
        public async Task Test11RegisterUserAsync()
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
        public async Task Test12RegisterUserAsync()
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
        public async Task Test13RegisterUserAsync()
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

        #region Good
        [Fact]
        public async Task Test14RegisterUserAsync()
        {
            User user = new User
            {
                FirstName = "Roman",
                LastName = "Kwiatkowski",
                Email = "Roman.Kwiatkowski@example.com",
                Role = "Customer",
                Password = "Password12345",
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
