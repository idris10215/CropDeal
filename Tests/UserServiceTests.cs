using Moq;
using NUnit.Framework;
using CropDeal.Services;
using CropDeal.Repositories;
using CropDeal.Models;
using Microsoft.Extensions.Configuration;

namespace CropDeal.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepository>? _mockRepo;
        private Mock<IConfiguration>? _mockConfig;
        private UserService? _userService;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IUserRepository>();
            _mockConfig = new Mock<IConfiguration>();

            _mockConfig.Setup(c => c["Jwt:Key"]).Returns("ThisIsMySuperSecretKey123456789_Longer_For_Security");
            _mockConfig.Setup(c => c["Jwt:Issuer"]).Returns("CropDealIssuer");
            _mockConfig.Setup(c => c["Jwt:Audience"]).Returns("CropDealAudience");

            _userService = new UserService(_mockRepo.Object, _mockConfig.Object);
        }

        [Test]
        public async Task RegisterUserAsync_ShouldHashPasswordAndSaveUser()
        {
            var dto = new RegisterDto 
            { 
                FullName = "Idris", 
                Email = "idris@example.com", 
                Password = "Password123", 
                Role = "Farmer" 
            };

            var result = await _userService!.RegisterUserAsync(dto);

            _mockRepo!.Verify(r => r.AddUserAsync(It.IsAny<User>()), Times.Once);
            
            _mockRepo.Verify(r => r.AddUserAsync(It.Is<User>(u => u.PasswordHash != "Password123")));
            
            Assert.That(result, Is.EqualTo("User registered successfully!"));
        }

        [Test]
        public async Task LoginAsync_WithValidCredentials_ShouldReturnToken()
        {
            var plainPassword = "Password123";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainPassword);
            
            var fakeUser = new User 
            { 
                Email = "idris@example.com", 
                PasswordHash = hashedPassword,
                FullName = "Idris",
                Role = "Farmer"
            };

            var loginDto = new LoginDto { Email = "idris@example.com", Password = plainPassword };

            _mockRepo!.Setup(r => r.GetUserByEmailAsync("idris@example.com")).ReturnsAsync(fakeUser);

            var token = await _userService!.LoginAsync(loginDto);

            Assert.IsNotNull(token);
            Assert.IsNotEmpty(token);
        }

        [Test]
        public async Task LoginAsync_WithWrongPassword_ShouldReturnNull()
        {
            // Arrange
            // Use a real BCrypt hash of any string so the library doesn't crash
            var validLookingHash = BCrypt.Net.BCrypt.HashPassword("Anything"); 
            
            var fakeUser = new User { Email = "idris@example.com", PasswordHash = validLookingHash };
            var loginDto = new LoginDto { Email = "idris@example.com", Password = "WrongPassword" };

            _mockRepo!.Setup(r => r.GetUserByEmailAsync("idris@example.com")).ReturnsAsync(fakeUser);

            // Act
            var result = await _userService!.LoginAsync(loginDto);

            // Assert
            Assert.IsNull(result);
        }
    }
}