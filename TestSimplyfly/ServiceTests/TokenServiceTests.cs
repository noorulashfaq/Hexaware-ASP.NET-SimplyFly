using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using SimplyFlyServer.Interface;
using SimplyFlyServer.Service;
using System;
using System.Threading.Tasks;

namespace TestSimplyfly.ServiceTests
{
	[TestFixture]
	public class TokenServiceTests
	{
		private ITokenService _tokenService;

		[SetUp]
		public void Setup()
		{
			var mockConfig = new ConfigurationBuilder()
				.AddInMemoryCollection(new[]
				{
					new KeyValuePair<string, string>("Keys:JwtToken", "ThisIsASecretKeyForJwtToken12345")
				})
				.Build();

			_tokenService = new TokenService(mockConfig);
		}

		[Test]
		public async Task GenerateToken_ShouldReturn_ValidJwtToken()
		{
			int userId = 1;
			string userName = "TestUser";

			var token = await _tokenService.GenerateToken(userId, userName);

			Assert.IsNotNull(token);
			Assert.IsTrue(token.Split('.').Length == 3);
		}

		[Test]
		public void GenerateToken_ShouldThrowException_WhenKeyIsInvalid()
		{
			var ex = Assert.Throws<ArgumentException>(() =>
			{
				var invalidConfig = new ConfigurationBuilder()
					.AddInMemoryCollection(new[]
					{
				new KeyValuePair<string, string>("Keys:JwtToken", "")
					})
					.Build();

				var tokenService = new TokenService(invalidConfig); 
			});

			Assert.That(ex.Message, Does.Contain("IDX10703").Or.Contain("key length is zero"));
		}

	}
}
