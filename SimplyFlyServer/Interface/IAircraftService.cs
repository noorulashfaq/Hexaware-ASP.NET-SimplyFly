using SimplyFlyServer.Models;
using SimplyFlyServer.Models.DTOs;

namespace SimplyFlyServer.Interface
{
    public interface IAircraftService
    {
        Task<AircraftResponse> AddAircraft(AircraftRequest flight);
        Task<AircraftResponse> UpdateAircraft(int id, AircraftRequest flight);
        Task<bool> DeleteAircraft(int id);
        Task<AircraftResponse> GetAircraftById(int id);
        Task<IEnumerable<AircraftResponse>> GetAllAircrafts();
    }
}
