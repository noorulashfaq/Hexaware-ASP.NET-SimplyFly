using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SimplyFlyServer.Context;
using SimplyFlyServer.Models;
using SimplyFlyServer.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TestSimplyfly.RepositoryTests
{
	[TestFixture]
	public class AircraftRepositoryTests
	{
		private SimplyFlyContext _context;
		private AircraftRepository _aircraftRepo;

		[SetUp]
		public void Setup()
		{
			var options = new DbContextOptionsBuilder<SimplyFlyContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			_context = new SimplyFlyContext(options);
			_aircraftRepo = new AircraftRepository(_context);
		}

		private Airline CreateAirline()
		{
			var airline = new Airline
			{
				AirlineName = "TestAir",
				AirlineCode = "TA001",
				Country = "India",
				OwnerId = 1
			};
			_context.Airlines.Add(airline);
			_context.SaveChanges();
			return airline;
		}

		private Aircraft GetMockAircraft(int id = 1, string name = "JetX")
		{
			var airline = CreateAirline();
			return new Aircraft
			{
				AircraftId = id,
				AircraftName = name,
				AircraftNumber = $"AX{id:D3}",
				TotalSeats = 150 + id,
				FlightStatus = "Active",
				AirlineId = airline.AirlineId,
				Airline = airline
			};
		}

		[Test]
		public async Task Add_ShouldInsertAircraft()
		{
			var aircraft = GetMockAircraft();
			var result = await _aircraftRepo.Add(aircraft);

			Assert.IsNotNull(result);
			Assert.That(result.AircraftName, Is.EqualTo("JetX"));
		}

		[Test]
		public async Task GetAll_ShouldReturnAllAircrafts()
		{
			await _aircraftRepo.Add(GetMockAircraft(1, "PlaneA"));
			await _aircraftRepo.Add(GetMockAircraft(2, "PlaneB"));

			var result = await _aircraftRepo.GetAll();

			Assert.That(result.Count(), Is.EqualTo(2));
		}

		[Test]
		public async Task GetById_ShouldReturnCorrectAircraft()
		{
			var aircraft = GetMockAircraft(99, "SpecialJet");
			await _aircraftRepo.Add(aircraft);

			var result = await _aircraftRepo.GetById(99);

			Assert.That(result.AircraftName, Is.EqualTo("SpecialJet"));
		}

		[Test]
		public void GetById_InvalidId_ShouldThrowException()
		{
			var ex = Assert.ThrowsAsync<Exception>(async () => await _aircraftRepo.GetById(999));
			Assert.That(ex.Message, Is.EqualTo("Flight not found"));
		}

		[Test]
		public async Task Update_ShouldModifyAircraft()
		{
			var aircraft = GetMockAircraft();
			await _aircraftRepo.Add(aircraft);

			var updated = new Aircraft
			{
				AircraftId = aircraft.AircraftId,
				AircraftName = "UpdatedJet",
				AircraftNumber = "AX999",
				TotalSeats = 200,
				FlightStatus = "Maintenance",
				AirlineId = aircraft.AirlineId
			};

			var result = await _aircraftRepo.Update(aircraft.AircraftId, updated);

			Assert.That(result.AircraftName, Is.EqualTo("UpdatedJet"));
			Assert.That(result.FlightStatus, Is.EqualTo("Maintenance"));
		}

		[Test]
		public async Task Delete_ShouldRemoveAircraft()
		{
			var aircraft = GetMockAircraft();
			await _aircraftRepo.Add(aircraft);

			var deleted = await _aircraftRepo.Delete(aircraft.AircraftId);

			Assert.That(deleted.AircraftId, Is.EqualTo(aircraft.AircraftId));
			Assert.ThrowsAsync<Exception>(async () => await _aircraftRepo.GetById(aircraft.AircraftId));
		}
	}
}
