using NUnit.Framework;
using Moq;
using SimplyFlyServer.Interface;
using SimplyFlyServer.Models;
using SimplyFlyServer.Models.DTOs;
using SimplyFlyServer.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestSimplyfly.ServiceTests
{
	[TestFixture]
	public class AircraftServiceTests
	{
		private Mock<IRepository<int, Aircraft>> _aircraftRepoMock;
		private AircraftService _aircraftService;

		[SetUp]
		public void Setup()
		{
			_aircraftRepoMock = new Mock<IRepository<int, Aircraft>>();
			_aircraftService = new AircraftService(_aircraftRepoMock.Object);
		}

		[Test]
		public async Task AddAircraft_ShouldReturnAircraftResponse()
		{
			var request = new AircraftRequest
			{
				AircraftName = "Boeing 737",
				AircraftNumber = "BOE123",
				TotalSeats = 180,
				FlightStatus = "Active",
				AirlineId = 1
			};

			var created = new Aircraft
			{
				AircraftId = 1,
				AircraftName = "Boeing 737",
				AircraftNumber = "BOE123",
				TotalSeats = 180,
				FlightStatus = "Active",
				AirlineId = 1
			};

			_aircraftRepoMock.Setup(repo => repo.Add(It.IsAny<Aircraft>()))
				.ReturnsAsync(created);

			var result = await _aircraftService.AddAircraft(request);

			Assert.NotNull(result);
			Assert.AreEqual(request.AircraftName, result.AircraftName);
			Assert.AreEqual(created.AircraftId, result.AircraftId);
		}

		[Test]
		public async Task UpdateAircraft_ShouldReturnUpdatedAircraftResponse()
		{
			var request = new AircraftRequest
			{
				AircraftName = "Airbus A320",
				AircraftNumber = "AIR456",
				TotalSeats = 160,
				FlightStatus = "Maintenance",
				AirlineId = 2
			};

			var existing = new Aircraft
			{
				AircraftId = 2,
				AircraftName = "Old Name",
				AircraftNumber = "OLD123",
				TotalSeats = 100,
				FlightStatus = "Inactive",
				AirlineId = 2
			};

			_aircraftRepoMock.Setup(repo => repo.GetById(2)).ReturnsAsync(existing);
			_aircraftRepoMock.Setup(repo => repo.Update(2, It.IsAny<Aircraft>()))
				.ReturnsAsync((int id, Aircraft a) => a);

			var result = await _aircraftService.UpdateAircraft(2, request);

			Assert.NotNull(result);
			Assert.AreEqual(request.AircraftName, result.AircraftName);
			Assert.AreEqual(request.TotalSeats, result.TotalSeats);
		}

		[Test]
		public async Task DeleteAircraft_WhenAircraftExists_ShouldReturnTrue()
		{
			var aircraft = new Aircraft { AircraftId = 3 };
			_aircraftRepoMock.Setup(r => r.GetById(3)).ReturnsAsync(aircraft);
			//_aircraftRepoMock.Setup(r => r.Delete(3)).Returns(true);

			var result = await _aircraftService.DeleteAircraft(3);

			Assert.IsTrue(result);
		}

		[Test]
		public async Task DeleteAircraft_WhenAircraftDoesNotExist_ShouldReturnFalse()
		{
			_aircraftRepoMock.Setup(r => r.GetById(4)).ReturnsAsync((Aircraft)null);

			var result = await _aircraftService.DeleteAircraft(4);

			Assert.IsFalse(result);
		}

		[Test]
		public async Task GetAircraftById_ShouldReturnAircraftResponse()
		{
			var aircraft = new Aircraft
			{
				AircraftId = 5,
				AircraftName = "SpiceJet",
				AircraftNumber = "SPJ111",
				TotalSeats = 150,
				FlightStatus = "Active",
				AirlineId = 3
			};

			_aircraftRepoMock.Setup(r => r.GetById(5)).ReturnsAsync(aircraft);

			var result = await _aircraftService.GetAircraftById(5);

			Assert.NotNull(result);
			Assert.AreEqual("SpiceJet", result.AircraftName);
		}

		[Test]
		public async Task GetAllAircrafts_ShouldReturnAllAircraftResponses()
		{
			var aircraftList = new List<Aircraft>
			{
				new Aircraft { AircraftId = 1, AircraftName = "Jet A", AircraftNumber = "JA01", TotalSeats = 100, FlightStatus = "Active", AirlineId = 1 },
				new Aircraft { AircraftId = 2, AircraftName = "Jet B", AircraftNumber = "JB02", TotalSeats = 200, FlightStatus = "Active", AirlineId = 2 }
			};

			_aircraftRepoMock.Setup(r => r.GetAll()).ReturnsAsync(aircraftList);

			var result = await _aircraftService.GetAllAircrafts();

			Assert.AreEqual(2, result.Count());
		}

		[Test]
		public void GetAircraftById_WhenAircraftDoesNotExist_ShouldThrowException()
		{
			_aircraftRepoMock.Setup(r => r.GetById(It.IsAny<int>())).ReturnsAsync((Aircraft?)null);

			var ex = Assert.ThrowsAsync<Exception>(() => _aircraftService.GetAircraftById(10));
			Assert.That(ex?.Message, Is.EqualTo("Flight not found"));
		}

		[Test]
		public void UpdateAircraft_WhenAircraftDoesNotExist_ShouldThrowException()
		{
			var request = new AircraftRequest
			{
				AircraftName = "Test",
				AircraftNumber = "123",
				TotalSeats = 100,
				FlightStatus = "Ready",
				AirlineId = 1
			};

			_aircraftRepoMock.Setup(r => r.GetById(It.IsAny<int>())).ReturnsAsync((Aircraft?)null);

			var ex = Assert.ThrowsAsync<Exception>(() => _aircraftService.UpdateAircraft(5, request));
			Assert.That(ex?.Message, Is.EqualTo("Flight not found"));
		}

		[Test]
		public void AddAircraft_WhenRepositoryReturnsNull_ShouldThrowException()
		{
			var request = new AircraftRequest
			{
				AircraftName = "Test",
				AircraftNumber = "123",
				TotalSeats = 100,
				FlightStatus = "Ready",
				AirlineId = 1
			};

			_aircraftRepoMock.Setup(r => r.Add(It.IsAny<Aircraft>())).ReturnsAsync((Aircraft?)null);

			var ex = Assert.ThrowsAsync<Exception>(() => _aircraftService.AddAircraft(request));
			Assert.That(ex?.Message, Is.EqualTo("Failed to create flight"));
		}

	}
}
