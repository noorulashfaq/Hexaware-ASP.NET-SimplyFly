using AutoMapper;
using SimplyFlyServer.Models.DTOs;
using SimplyFlyServer.Models;

namespace SimplyFlyServer.Mapper
{
    public class BookingMapper : Profile
    {
        public BookingMapper() 
        {
            CreateMap<Booking, BookingRequest>().ReverseMap();
            CreateMap<Booking, BookingResponse>();
           
        }
    }
}
