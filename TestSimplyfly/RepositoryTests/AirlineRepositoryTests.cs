using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SimplyFlyServer.Context;
using SimplyFlyServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestSimplyfly.RepositoryTests
{
	[TestFixture]
	public class AirlineRepositoryTests
	{
		private SimplyFlyContext _context;

		[SetUp]
		public void Setup()
		{
			var options = new DbContextOptionsBuilder<SimplyFlyContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString())
				.Options;

			_context = new SimplyFlyContext(options);
		}

		private User CreateOwner()
		{
			var user = new User
			{
				UserName = "airline_owner",
				FirstName = "Alex",
				LastName = "Jet",
				PhoneNumber = "9876543210",
				Password = new byte[] { 1, 2, 3 },
				HashKey = new byte[] { 4, 5, 6 },
				Address = "Sky Street",
				Role = User.Usertype.Admin,
				CreatedDate = DateTime.Now
			};

			_context.Users.Add(user);
			_context.SaveChanges();
			return user;
		}

		[Test]
		public async Task CanCreateAndRetrieveAirline()
		{
			var owner = CreateOwner();

			var airline = new Airline
			{
				AirlineName = "FlyHigh",
				AirlineCode = "FH123",
				OwnerId = owner.UserId,
				Country = "India"
			};

			_context.Airlines.Add(airline);
			await _context.SaveChangesAsync();

			var retrieved = await _context.Airlines.FirstOrDefaultAsync(a => a.AirlineCode == "FH123");

			Assert.IsNotNull(retrieved);
			Assert.AreEqual("FlyHigh", retrieved.AirlineName);
			Assert.AreEqual("India", retrieved.Country);
		}

		[Test]
		public async Task Airline_ShouldHaveOwnerNavigation()
		{
			var owner = CreateOwner();

			var airline = new Airline
			{
				AirlineName = "JetKing",
				AirlineCode = "JK999",
				OwnerId = owner.UserId,
				Country = "UAE"
			};

			_context.Airlines.Add(airline);
			await _context.SaveChangesAsync();

			var result = await _context.Airlines
				.Include(a => a.Owner)
				.FirstOrDefaultAsync(a => a.AirlineCode == "JK999");

			Assert.IsNotNull(result.Owner);
			Assert.AreEqual("airline_owner", result.Owner.UserName);
		}

		[Test]
		public async Task Airline_CanHaveMultipleAircrafts()
		{
			var owner = CreateOwner();

			var airline = new Airline
			{
				AirlineName = "Sky Fleet",
				AirlineCode = "SF101",
				OwnerId = owner.UserId,
				Country = "USA"
			};

			_context.Airlines.Add(airline);
			await _context.SaveChangesAsync();

			var aircraft1 = new Aircraft
			{
				AircraftName = "SkyBird 1",
				AircraftNumber = "SB001",
				TotalSeats = 180,
				AirlineId = airline.AirlineId
			};

			var aircraft2 = new Aircraft
			{
				AircraftName = "SkyBird 2",
				AircraftNumber = "SB002",
				TotalSeats = 200,
				AirlineId = airline.AirlineId
			};

			_context.Aircrafts.AddRange(aircraft1, aircraft2);
			await _context.SaveChangesAsync();

			var result = await _context.Airlines
				.Include(a => a.Aircrafts)
				.FirstOrDefaultAsync(a => a.AirlineCode == "SF101");

			Assert.That(result.Aircrafts.Count, Is.EqualTo(2));
			Assert.That(result.Aircrafts.Any(a => a.AircraftName == "SkyBird 1"));
		}
	}
}
