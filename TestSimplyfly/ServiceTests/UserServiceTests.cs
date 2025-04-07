using Moq;
using NUnit.Framework;
using SimplyFlyServer.Interface;
using SimplyFlyServer.Models;
using SimplyFlyServer.Models.DTOs;
using SimplyFlyServer.Service;
using System.Text;

namespace TestSimplyfly.ServiceTests
{
	[TestFixture]
	public class UserServiceTests
	{
		private Mock<IRepository<int, User>> _userRepoMock;
		private IUserService _userService;

		[SetUp]
		public void Setup()
		{
			_userRepoMock = new Mock<IRepository<int, User>>();
			_userService = new UserService(_userRepoMock.Object);
		}

		[Test]
		public async Task AddUser_ShouldReturnUserResponse_WhenUserIsCreated()
		{
			var request = new UserRequest
			{
				UserName = "john_doe",
				FirstName = "John",
				LastName = "Doe",
				Password = "password123",
				PhoneNumber = "1234567890",
				Address = "NY"
			};

			_userRepoMock.Setup(x => x.Add(It.IsAny<User>()))
						 .ReturnsAsync((User u) => { u.UserId = 1; return u; });

			var result = await _userService.AddUser(request);

			Assert.IsNotNull(result);
			Assert.That(result.UserId, Is.EqualTo(1));
		}

		[Test]
		public async Task UpdateUser_ShouldUpdateAndReturnUserResponse()
		{
			var existingUser = new User
			{
				UserId = 1,
				UserName = "john_doe",
				FirstName = "John",
				LastName = "Doe",
				Password = Encoding.UTF8.GetBytes("oldpassword"),
				HashKey = Encoding.UTF8.GetBytes("key")
			};

			var request = new UserRequest
			{
				FirstName = "Johnny",
				Password = "newpass123"
			};

			_userRepoMock.Setup(r => r.GetById(1)).ReturnsAsync(existingUser);
			_userRepoMock.Setup(r => r.Update(1, It.IsAny<User>())).ReturnsAsync((int id, User u) => u);

			var result = await _userService.UpdateUser(1, request);

			Assert.IsNotNull(result);
			Assert.That(result.UserId, Is.EqualTo(1));
			_userRepoMock.Verify(r => r.Update(1, It.Is<User>(u => u.FirstName == "Johnny")), Times.Once);
		}

		[Test]
		public void UpdateUser_ShouldThrowException_WhenUserNotFound()
		{
			_userRepoMock.Setup(r => r.GetById(It.IsAny<int>())).ReturnsAsync((User)null);

			var ex = Assert.ThrowsAsync<Exception>(() => _userService.UpdateUser(1, new UserRequest()));
			Assert.That(ex.Message, Is.EqualTo("User not found"));
		}

		[Test]
		public async Task DeleteUser_ShouldReturnTrue_WhenUserExists()
		{
			var existingUser = new User { UserId = 1 };
			_userRepoMock.Setup(r => r.GetById(1)).ReturnsAsync(existingUser);
			//_userRepoMock.Setup(r => r.Delete(1)).Returns(Task.CompletedTask);

			var result = await _userService.DeleteUser(1);

			Assert.IsTrue(result);
		}

		[Test]
		public void DeleteUser_ShouldThrowException_WhenUserNotFound()
		{
			_userRepoMock.Setup(r => r.GetById(It.IsAny<int>())).ReturnsAsync((User)null);

			var ex = Assert.ThrowsAsync<Exception>(() => _userService.DeleteUser(1));
			Assert.That(ex.Message, Is.EqualTo("User not found"));
		}
	}
}
