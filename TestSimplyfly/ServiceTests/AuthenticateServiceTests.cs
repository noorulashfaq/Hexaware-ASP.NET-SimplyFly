using Moq;
using NUnit.Framework;
using SimplyFlyServer.Interface;
using SimplyFlyServer.Models;
using SimplyFlyServer.Models.DTOs;
using SimplyFlyServer.Service;
using System.Security.Cryptography;
using System.Text;

namespace TestSimplyfly.ServiceTests
{
	[TestFixture]
	public class AuthenticationServiceTests
	{
		private Mock<IRepository<int, User>> _userRepoMock;
		private Mock<ITokenService> _tokenServiceMock;
		private AuthenticationService _authService;

		[SetUp]
		public void Setup()
		{
			_userRepoMock = new Mock<IRepository<int, User>>();
			_tokenServiceMock = new Mock<ITokenService>();
			_authService = new AuthenticationService(_userRepoMock.Object, _tokenServiceMock.Object);
		}

		private (byte[] passwordHash, byte[] key) HashPassword(string password)
		{
			using var hmac = new HMACSHA512();
			var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
			return (hash, hmac.Key);
		}

		[Test]
		public async Task Login_ValidCredentials_ReturnsLoginResponse()
		{
			// Arrange
			var password = "MyPassword123";
			var (hash, key) = HashPassword(password);

			var users = new List<User>
			{
				new User
				{
					UserId = 1,
					UserName = "testuser",
					Password = hash,
					HashKey = key
				}
			};

			_userRepoMock.Setup(r => r.GetAll()).ReturnsAsync(users);
			_tokenServiceMock.Setup(t => t.GenerateToken(1, "testuser")).ReturnsAsync("fake-jwt-token");

			var request = new LoginRequest { UserName = "testuser", Password = password };

			// Act
			var result = await _authService.Login(request);

			// Assert
			Assert.That(result.UserId, Is.EqualTo(1));
			Assert.That(result.Token, Is.EqualTo("fake-jwt-token"));
		}

		[Test]
		public void Login_UserNotFound_ThrowsUnauthorizedAccessException()
		{
			// Arrange
			_userRepoMock.Setup(r => r.GetAll()).ReturnsAsync(new List<User>());

			var request = new LoginRequest { UserName = "notfound", Password = "password" };

			// Act + Assert
			var ex = Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.Login(request));
			Assert.That(ex?.Message, Is.EqualTo("User not found"));
		}

		[Test]
		public void Login_InvalidPassword_ThrowsUnauthorizedAccessException()
		{
			// Arrange
			var correctPassword = "correctpass";
			var wrongPassword = "wrongpass";
			var (hash, key) = HashPassword(correctPassword);

			var users = new List<User>
			{
				new User
				{
					UserId = 1,
					UserName = "testuser",
					Password = hash,
					HashKey = key
				}
			};

			_userRepoMock.Setup(r => r.GetAll()).ReturnsAsync(users);

			var request = new LoginRequest { UserName = "testuser", Password = wrongPassword };

			// Act + Assert
			var ex = Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.Login(request));
			Assert.That(ex?.Message, Is.EqualTo("Invalid password"));
		}
	}
}
