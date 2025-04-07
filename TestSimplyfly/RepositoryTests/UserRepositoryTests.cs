using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SimplyFlyServer.Context;
using SimplyFlyServer.Models;
using SimplyFlyServer.Repository;

namespace TestSimplyfly.RepositoryTests
{
	[TestFixture]
	public class UserRepositoryTests
	{
		private SimplyFlyContext _context;
		private UserRepository _userRepository;

		[SetUp]
		public void Setup()
		{
			var options = new DbContextOptionsBuilder<SimplyFlyContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Ensures isolation per test
				.Options;

			_context = new SimplyFlyContext(options);
			_userRepository = new UserRepository(_context);
		}

		private User GetMockUser(int id = 1, string username = "noorul_ashfaq")
		{
			return new User
			{
				UserId = id,
				UserName = username,
				FirstName = "Noorul",
				LastName = "Ashfaq",
				Password = new byte[] { 1, 2, 3 },
				HashKey = new byte[] { 4, 5, 6 },
				PhoneNumber = "1234567890",
				Address = "123 Main St",
				Role = User.Usertype.Passenger,
				CreatedDate = DateTime.Now
			};
		}

		[Test]
		public async Task Add_ShouldAddUserSuccessfully()
		{
			var user = GetMockUser();
			var result = await _userRepository.Add(user);

			Assert.IsNotNull(result);
			Assert.AreEqual(user.UserName, result.UserName);
		}

		[Test]
		public async Task GetAll_ShouldReturnAllUsers()
		{
			var user1 = GetMockUser(1, "noorul_ashfaq");
			var user2 = GetMockUser(2, "sasi_sundaresan");

			await _userRepository.Add(user1);
			await _userRepository.Add(user2);

			var result = await _userRepository.GetAll();

			Assert.That(result.Count(), Is.EqualTo(2));
		}

		[Test]
		public async Task GetById_ShouldReturnCorrectUser()
		{
			var user = GetMockUser();
			await _userRepository.Add(user);

			var result = await _userRepository.GetById(user.UserId);

			Assert.IsNotNull(result);
			Assert.AreEqual(user.UserId, result.UserId);
		}

		[Test]
		public void GetById_ShouldThrowException_IfUserNotFound()
		{
			var ex = Assert.ThrowsAsync<Exception>(async () => await _userRepository.GetById(100));
			Assert.That(ex.Message, Is.EqualTo("Users not found"));
		}

		[Test]
		public async Task Update_ShouldModifyUser()
		{
			var user = GetMockUser();
			await _userRepository.Add(user);

			var updatedUser = new User
			{
				UserId = user.UserId,
				UserName = user.UserName,
				FirstName = "UpdatedName",
				LastName = user.LastName,
				Password = user.Password,
				HashKey = user.HashKey,
				PhoneNumber = "9999999999",
				Address = user.Address,
				Role = user.Role,
				CreatedDate = user.CreatedDate,
				AirlineAssociated = user.AirlineAssociated
			};

			var result = await _userRepository.Update(user.UserId, updatedUser);

			Assert.That(result.FirstName, Is.EqualTo("UpdatedName"));
			Assert.That(result.PhoneNumber, Is.EqualTo("9999999999"));
		}

		[Test]
		public async Task Delete_ShouldRemoveUser()
		{
			var user = GetMockUser();
			await _userRepository.Add(user);

			var deleted = await _userRepository.Delete(user.UserId);

			Assert.AreEqual(user.UserId, deleted.UserId);
			Assert.ThrowsAsync<Exception>(async () => await _userRepository.GetById(user.UserId));
		}
	}
}
