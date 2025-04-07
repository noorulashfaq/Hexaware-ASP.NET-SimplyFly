using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SimplyFlyServer.Context;
using SimplyFlyServer.Models;
using SimplyFlyServer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestSimplyfly.RepositoryTests
{
	[TestFixture]
	public class FlightRepositoryTests
	{
		private SimplyFlyContext _context;
		private FlightRepository _flightRepo;

		[SetUp]
		public void Setup()
		{
			var options = new DbContextOptionsBuilder<SimplyFlyContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			_context = new SimplyFlyContext(options);
			_flightRepo = new FlightRepository(_context);
		}

		private Airline CreateAirline()
		{
			var airline = new Airline
			{
				AirlineName = "SkyJet",
				AirlineCode = "SJ123",
				Country = "USA",
				OwnerId = 1
			};
			_context.Airlines.Add(airline);
			_context.SaveChanges();
			return airline;
		}

		private Aircraft CreateAircraft(Airline airline)
		{
			var aircraft = new Aircraft
			{
				AircraftName = "Boeing 737",
				AircraftNumber = "B737",
				TotalSeats = 180,
				FlightStatus = "Active",
				AirlineId = airline.AirlineId,
				Airline = airline
			};
			_context.Aircrafts.Add(aircraft);
			_context.SaveChanges();
			return aircraft;
		}

		private FlightRoute CreateFlightRoute()
		{
			var route = new FlightRoute
			{
				Source = "Chennai",
				Destination = "Delhi",
			};
			_context.FlightRoutes.Add(route);
			_context.SaveChanges();
			return route;
		}

		private Flight CreateFlight()
		{
			var airline = CreateAirline();
			var aircraft = CreateAircraft(airline);
			var route = CreateFlightRoute();

			var flight = new Flight
			{
				AircraftId = aircraft.AircraftId,
				Aircraft = aircraft,
				RouteId = route.RouteId,
				FlightRoute = route,
				AirlineId = airline.AirlineId,
				Airline = airline,
				DepartureTime = DateTime.Now.AddHours(1),
				ArrivalTime = DateTime.Now.AddHours(3.5),
				BaggageInfo = "15kg",
				AvailableSeats = 160
			};

			_context.Flights.Add(flight);
			_context.SaveChanges();
			return flight;
		}

		[Test]
		public async Task GetAll_ShouldReturnFlightsWithRelations()
		{
			CreateFlight();
			CreateFlight();

			var result = await _flightRepo.GetAll();

			Assert.That(result.Count(), Is.EqualTo(2));
			foreach (var flight in result)
			{
				Assert.IsNotNull(flight.Aircraft);
				Assert.IsNotNull(flight.Aircraft.Airline);
				Assert.IsNotNull(flight.FlightRoute);
			}
		}

		[Test]
		public async Task GetById_ShouldReturnCorrectFlight()
		{
			var inserted = CreateFlight();

			var result = await _flightRepo.GetById(inserted.FlightId);

			Assert.IsNotNull(result);
			Assert.That(result.FlightId, Is.EqualTo(inserted.FlightId));
		}

		[Test]
		public void GetById_InvalidId_ShouldThrowException()
		{
			var ex = Assert.ThrowsAsync<Exception>(async () => await _flightRepo.GetById(999));
			Assert.That(ex.Message, Is.EqualTo("Flying routes not found"));
		}
	}
}
