using AutoMapper;
using SimplyFlyServer.Models.DTOs;
using SimplyFlyServer.Models;

namespace SimplyFlyServer.Mapper
{
    public class CancellationMapper : Profile
    {
        public CancellationMapper()
        {
            CreateMap<Cancellation, CancellationRequest>().ReverseMap();
            CreateMap<Cancellation, CancellationResponse>();
        }
    }
}
