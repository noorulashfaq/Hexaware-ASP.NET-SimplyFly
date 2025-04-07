using Moq;
using NUnit.Framework;
using SimplyFlyServer.Interface;
using SimplyFlyServer.Models;
using SimplyFlyServer.Models.DTOs;
using SimplyFlyServer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestSimplyfly.ServiceTests
{
	[TestFixture]
	public class FlightServiceTests
	{
		private Mock<IRepository<int, Flight>> _mockRepo;
		private FlightService _flightService;

		[SetUp]
		public void Setup()
		{
			_mockRepo = new Mock<IRepository<int, Flight>>();
			_flightService = new FlightService(_mockRepo.Object);
		}

		[Test]
		public async Task AddFlight_ShouldReturnFlightResponse()
		{
			var request = new FlightRequest
			{
				AircraftId = 1,
				RouteId = 1,
				AirlineId = 2,
				DepartureTime = DateTime.Now,
				ArrivalTime = DateTime.Now.AddHours(2),
				BaggageInfo = "20kg",
				AvailableSeats = 100
			};

			var flight = new Flight
			{
				FlightId = 1,
				AircraftId = 1,
				RouteId = 1,
				AirlineId = 2,
				DepartureTime = request.DepartureTime,
				ArrivalTime = request.ArrivalTime,
				BaggageInfo = request.BaggageInfo,
				AvailableSeats = request.AvailableSeats,
				Aircraft = new Aircraft { AirlineId = 2 }
			};

			_mockRepo.Setup(repo => repo.Add(It.IsAny<Flight>())).ReturnsAsync(flight);

			var result = await _flightService.AddFlight(request);

			Assert.That(result.FlightId, Is.EqualTo(1));
			Assert.That(result.AirlineId, Is.EqualTo(2));
		}

		[Test]
		public void AddFlight_ShouldThrowException_OnDbUpdateException()
		{
			var request = new FlightRequest { AircraftId = 1 };
			_mockRepo.Setup(repo => repo.Add(It.IsAny<Flight>())).ThrowsAsync(new Exception("DB Error"));

			Assert.ThrowsAsync<Exception>(() => _flightService.AddFlight(request));
		}

		[Test]
		public async Task UpdateFlight_ShouldUpdateAndReturnResponse()
		{
			var existing = new Flight { FlightId = 1, AircraftId = 1 };
			var request = new FlightRequest { AircraftId = 2 };

			_mockRepo.Setup(r => r.GetById(1)).ReturnsAsync(existing);
			_mockRepo.Setup(r => r.Update(1, It.IsAny<Flight>())).ReturnsAsync(existing);

			var result = await _flightService.UpdateFlight(1, request);

			Assert.That(result.AircraftId, Is.EqualTo(2));
		}

		[Test]
		public void UpdateFlight_ShouldThrowException_IfNotFound()
		{
			_mockRepo.Setup(r => r.GetById(1)).ReturnsAsync((Flight)null);
			var request = new FlightRequest { AircraftId = 2 };

			Assert.ThrowsAsync<Exception>(() => _flightService.UpdateFlight(1, request));
		}

		[Test]
		public async Task DeleteFlight_ShouldReturnTrue_IfExists()
		{
			_mockRepo.Setup(r => r.GetById(1)).ReturnsAsync(new Flight { FlightId = 1 });
			//_mockRepo.Setup(r => r.Delete(1)).Returns(Task.CompletedTask);

			var result = await _flightService.DeleteFlight(1);

			Assert.That(result, Is.True);
		}

		[Test]
		public async Task DeleteFlight_ShouldReturnFalse_IfNotExists()
		{
			_mockRepo.Setup(r => r.GetById(1)).ReturnsAsync((Flight)null);

			var result = await _flightService.DeleteFlight(1);

			Assert.That(result, Is.False);
		}

		[Test]
		public async Task GetAllFlights_ShouldReturnAll()
		{
			var flights = new List<Flight> { new Flight { FlightId = 1 }, new Flight { FlightId = 2 } };
			_mockRepo.Setup(r => r.GetAll()).ReturnsAsync(flights);

			var result = await _flightService.GetAllFlights();

			Assert.That(result.Count(), Is.EqualTo(2));
		}

		[Test]
		public async Task GetFlightById_ShouldReturnCorrectFlight()
		{
			_mockRepo.Setup(r => r.GetById(1)).ReturnsAsync(new Flight { FlightId = 1 });

			var result = await _flightService.GetFlightById(1);

			Assert.That(result.FlightId, Is.EqualTo(1));
		}

		[Test]
		public void GetFlightById_ShouldThrow_IfNotFound()
		{
			_mockRepo.Setup(r => r.GetById(1)).ReturnsAsync((Flight)null);

			Assert.ThrowsAsync<Exception>(() => _flightService.GetFlightById(1));
		}

		[Test]
		public async Task GetFlightsByFilter_ShouldReturnFilteredFlights()
		{
			var flights = new List<Flight>
			{
				new Flight { FlightId = 1, Aircraft = new Aircraft { AirlineId = 5 }, DepartureTime = DateTime.Today, ArrivalTime = DateTime.Today.AddHours(2) },
				new Flight { FlightId = 2, Aircraft = new Aircraft { AirlineId = 9 }, DepartureTime = DateTime.Today, ArrivalTime = DateTime.Today.AddHours(3) }
			};

			_mockRepo.Setup(r => r.GetAll()).ReturnsAsync(flights);

			var filter = new FlightFilterRequest
			{
				Filters = new FlightFilter { AirlineId = 5, DepartureTime = DateTime.Today }
			};

			var result = await _flightService.GetFlightsByFilter(filter);

			Assert.That(result.Count(), Is.EqualTo(1));
			Assert.That(result.First().FlightId, Is.EqualTo(1));
		}
	}
}
