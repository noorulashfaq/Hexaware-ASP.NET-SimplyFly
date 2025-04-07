using Moq;
using NUnit.Framework;
using SimplyFlyServer.Interface;
using SimplyFlyServer.Models;
using SimplyFlyServer.Models.DTOs;
using SimplyFlyServer.Service;

namespace TestSimplyfly.ServiceTests
{
	[TestFixture]
	public class AirlineServiceTests
	{
		private Mock<IRepository<int, Airline>> _airlineRepoMock;
		private AirlineService _airlineService;

		[SetUp]
		public void Setup()
		{
			_airlineRepoMock = new Mock<IRepository<int, Airline>>();
			_airlineService = new AirlineService(_airlineRepoMock.Object);
		}

		[Test]
		public async Task AddAirline_ValidRequest_ReturnsAirlineResponse()
		{
			var request = new AirlineRequest
			{
				AirlineName = "SkyHigh",
				AirlineCode = "SH123",
				Country = "India",
				OwnerId = 10
			};

			var addedAirline = new Airline
			{
				AirlineId = 1,
				AirlineName = request.AirlineName,
				AirlineCode = request.AirlineCode,
				Country = request.Country,
				OwnerId = request.OwnerId
			};

			_airlineRepoMock.Setup(repo => repo.Add(It.IsAny<Airline>())).ReturnsAsync(addedAirline);

			var result = await _airlineService.AddAirline(request);

			Assert.That(result.AirlineId, Is.EqualTo(1));
			Assert.That(result.AirlineName, Is.EqualTo("SkyHigh"));
		}

		[Test]
		public void AddAirline_WhenRepositoryReturnsNull_ThrowsException()
		{
			var request = new AirlineRequest
			{
				AirlineName = "FailTest",
				AirlineCode = "FT001",
				Country = "TestLand",
				OwnerId = 1
			};

			_airlineRepoMock.Setup(r => r.Add(It.IsAny<Airline>())).ReturnsAsync((Airline?)null);

			var ex = Assert.ThrowsAsync<Exception>(() => _airlineService.AddAirline(request));
			Assert.That(ex?.Message, Is.EqualTo("Failed to create airline"));
		}

		[Test]
		public async Task UpdateAirline_ValidId_UpdatesAndReturnsResponse()
		{
			var id = 1;
			var request = new AirlineRequest
			{
				AirlineName = "Updated",
				AirlineCode = "UP123",
				Country = "UpdatedLand",
				OwnerId = 20
			};

			var existing = new Airline
			{
				AirlineId = id,
				AirlineName = "Old",
				AirlineCode = "OLD123",
				Country = "OldLand",
				OwnerId = 10
			};

			_airlineRepoMock.Setup(r => r.GetById(id)).ReturnsAsync(existing);
			_airlineRepoMock.Setup(r => r.Update(id, It.IsAny<Airline>())).ReturnsAsync(existing);

			var result = await _airlineService.UpdateAirline(id, request);

			Assert.That(result.AirlineName, Is.EqualTo("Updated"));
		}

		[Test]
		public void UpdateAirline_WhenNotFound_ThrowsException()
		{
			_airlineRepoMock.Setup(r => r.GetById(It.IsAny<int>())).ReturnsAsync((Airline?)null);

			var request = new AirlineRequest { AirlineName = "Update", AirlineCode = "U123", OwnerId = 1, Country = "Nowhere" };

			var ex = Assert.ThrowsAsync<Exception>(() => _airlineService.UpdateAirline(1, request));
			Assert.That(ex?.Message, Is.EqualTo("Airline not found"));
		}

		[Test]
		public async Task DeleteAirline_ValidId_ReturnsTrue()
		{
			var existing = new Airline { AirlineId = 1 };
			_airlineRepoMock.Setup(r => r.GetById(1)).ReturnsAsync(existing);
			//_airlineRepoMock.Setup(r => r.Delete(1)).Returns(Task.CompletedTask);

			var result = await _airlineService.DeleteAirline(1);

			Assert.IsTrue(result);
		}

		[Test]
		public async Task DeleteAirline_WhenNotFound_ReturnsFalse()
		{
			_airlineRepoMock.Setup(r => r.GetById(It.IsAny<int>())).ReturnsAsync((Airline?)null);

			var result = await _airlineService.DeleteAirline(100);
			Assert.IsFalse(result);
		}

		[Test]
		public async Task GetAirlineById_ValidId_ReturnsAirlineResponse()
		{
			var airline = new Airline
			{
				AirlineId = 1,
				AirlineName = "TestAir",
				AirlineCode = "TA001",
				Country = "India",
				OwnerId = 11
			};

			_airlineRepoMock.Setup(r => r.GetById(1)).ReturnsAsync(airline);

			var result = await _airlineService.GetAirlineById(1);

			Assert.That(result.AirlineName, Is.EqualTo("TestAir"));
		}

		[Test]
		public void GetAirlineById_WhenNotFound_ThrowsException()
		{
			_airlineRepoMock.Setup(r => r.GetById(It.IsAny<int>())).ReturnsAsync((Airline?)null);

			var ex = Assert.ThrowsAsync<Exception>(() => _airlineService.GetAirlineById(1));
			Assert.That(ex?.Message, Is.EqualTo("Airline not found"));
		}

		[Test]
		public async Task GetAllAirlines_ReturnsListOfAirlineResponses()
		{
			var list = new List<Airline>
			{
				new Airline { AirlineId = 1, AirlineName = "A1", AirlineCode = "A001", Country = "US", OwnerId = 1 },
				new Airline { AirlineId = 2, AirlineName = "A2", AirlineCode = "A002", Country = "UK", OwnerId = 2 }
			};

			_airlineRepoMock.Setup(r => r.GetAll()).ReturnsAsync(list);

			var result = await _airlineService.GetAllAirlines();

			Assert.That(result.Count(), Is.EqualTo(2));
		}
	}
}
