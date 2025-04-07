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
	public class RouteServiceTests
	{
		private Mock<IRepository<int, FlightRoute>> _routeRepositoryMock;
		private RouteService _routeService;

		[SetUp]
		public void Setup()
		{
			_routeRepositoryMock = new Mock<IRepository<int, FlightRoute>>();
			_routeService = new RouteService(_routeRepositoryMock.Object);
		}

		[Test]
		public async Task AddRoute_ShouldReturnFlightRouteResponse_WhenSuccessful()
		{
			var request = new FlightRouteRequest { Source = "Chennai", Destination = "Delhi" };
			var createdRoute = new FlightRoute { RouteId = 1, Source = "Chennai", Destination = "Delhi" };

			_routeRepositoryMock.Setup(r => r.Add(It.IsAny<FlightRoute>())).ReturnsAsync(createdRoute);

			var result = await _routeService.AddRoute(request);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.RouteId);
			Assert.AreEqual("Chennai", result.Source);
		}

		[Test]
		public void AddRoute_ShouldThrowArgumentNullException_WhenRequestIsNull()
		{
			Assert.ThrowsAsync<ArgumentNullException>(() => _routeService.AddRoute(null));
		}

		[Test]
		public async Task UpdateRoute_ShouldReturnUpdatedResponse_WhenRouteExists()
		{
			var routeId = 1;
			var request = new FlightRouteRequest { Source = "Chennai", Destination = "Mumbai" };
			var existingRoute = new FlightRoute { RouteId = 1, Source = "Chennai", Destination = "Delhi" };

			_routeRepositoryMock.Setup(r => r.GetById(routeId)).ReturnsAsync(existingRoute);
			_routeRepositoryMock.Setup(r => r.Update(routeId, It.IsAny<FlightRoute>())).ReturnsAsync(existingRoute);

			var result = await _routeService.UpdateRoute(routeId, request);

			Assert.IsNotNull(result);
			Assert.AreEqual("Mumbai", result.Destination);
		}

		[Test]
		public void UpdateRoute_ShouldThrowException_WhenRouteNotFound()
		{
			_routeRepositoryMock.Setup(r => r.GetById(1)).ReturnsAsync((FlightRoute)null);

			var request = new FlightRouteRequest { Source = "A", Destination = "B" };

			var ex = Assert.ThrowsAsync<Exception>(() => _routeService.UpdateRoute(1, request));
			Assert.AreEqual("Route not found", ex.Message);
		}

		[Test]
		public async Task DeleteRoute_ShouldReturnTrue_WhenRouteExists()
		{
			_routeRepositoryMock.Setup(r => r.GetById(1)).ReturnsAsync(new FlightRoute { RouteId = 1 });
			//_routeRepositoryMock.Setup(r => r.Delete(1)).ReturnsAsync(true);

			var result = await _routeService.DeleteRoute(1);

			Assert.IsTrue(result);
		}

		[Test]
		public async Task DeleteRoute_ShouldReturnFalse_WhenRouteDoesNotExist()
		{
			_routeRepositoryMock.Setup(r => r.GetById(1)).ReturnsAsync((FlightRoute)null);

			var result = await _routeService.DeleteRoute(1);

			Assert.IsFalse(result);
		}

		[Test]
		public async Task GetAllRoutes_ShouldReturnListOfFlightRouteResponses()
		{
			var routes = new List<FlightRoute>
			{
				new FlightRoute { RouteId = 1, Source = "Chennai", Destination = "Delhi" },
				new FlightRoute { RouteId = 2, Source = "Mumbai", Destination = "Kolkata" }
			};

			_routeRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(routes);

			var result = await _routeService.GetAllRoutes();

			Assert.AreEqual(2, result.Count());
		}

		[Test]
		public async Task GetRouteById_ShouldReturnRoute_WhenExists()
		{
			var route = new FlightRoute { RouteId = 1, Source = "A", Destination = "B" };
			_routeRepositoryMock.Setup(r => r.GetById(1)).ReturnsAsync(route);

			var result = await _routeService.GetRouteById(1);

			Assert.IsNotNull(result);
			Assert.AreEqual("A", result.Source);
		}

		[Test]
		public void GetRouteById_ShouldThrowException_WhenNotFound()
		{
			_routeRepositoryMock.Setup(r => r.GetById(1)).ReturnsAsync((FlightRoute)null);

			var ex = Assert.ThrowsAsync<Exception>(() => _routeService.GetRouteById(1));
			Assert.AreEqual("Route not found", ex.Message);
		}
	}
}
