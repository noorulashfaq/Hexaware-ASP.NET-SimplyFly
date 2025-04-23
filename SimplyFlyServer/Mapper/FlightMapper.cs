using AutoMapper;
using SimplyFlyServer.Models.DTOs;
using SimplyFlyServer.Models;

namespace SimplyFlyServer.Mapper
{
    public class FlightMapper : Profile
    {
        public FlightMapper()
        {
            CreateMap<Flight, FlightRequest>().ReverseMap();
            CreateMap<Flight, FlightResponse>();
        }
    }
}
