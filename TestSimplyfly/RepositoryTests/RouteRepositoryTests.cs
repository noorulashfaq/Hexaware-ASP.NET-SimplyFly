using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SimplyFlyServer.Context;
using SimplyFlyServer.Models;
using SimplyFlyServer.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SimplyFlyServer.Tests
{
	[TestFixture]
	public class RouteRepositoryTests
	{
		private SimplyFlyContext _context;
		private RouteRepository _routeRepository;

		[SetUp]
		public void Setup()
		{
			var options = new DbContextOptionsBuilder<SimplyFlyContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique per test
				.Options;

			_context = new SimplyFlyContext(options);
			_routeRepository = new RouteRepository(_context);
		}

		private FlightRoute CreateMockRoute(string source = "Chennai", string destination = "Delhi")
		{
			var route = new FlightRoute
			{
				Source = source,
				Destination = destination
			};
			_context.Routes.Add(route);
			_context.SaveChanges();
			return route;
		}

		[Test]
		public async Task GetAll_ShouldReturnAllRoutes()
		{
			CreateMockRoute("Chennai", "Mumbai");
			CreateMockRoute("Hyderabad", "Kolkata");

			var routes = await _routeRepository.GetAll();

			Assert.That(routes.Count(), Is.EqualTo(2));
		}

		[Test]
		public async Task GetById_ShouldReturnCorrectRoute()
		{
			var route = CreateMockRoute("Bangalore", "Goa");

			var result = await _routeRepository.GetById(route.RouteId);

			Assert.IsNotNull(result);
			Assert.That(result.Source, Is.EqualTo("Bangalore"));
			Assert.That(result.Destination, Is.EqualTo("Goa"));
		}

		[Test]
		public void GetById_InvalidId_ShouldThrowException()
		{
			var ex = Assert.ThrowsAsync<Exception>(async () => await _routeRepository.GetById(999));
			Assert.That(ex.Message, Is.EqualTo("Flight not found"));
		}
	}
}
