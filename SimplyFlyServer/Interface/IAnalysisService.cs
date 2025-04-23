using SimplyFlyServer.Models.DTOs;

namespace SimplyFlyServer.Interface
{
    public interface IAnalysisService
    {
        Task<AnalysisResponse> GetBookingStatistics();
    }
}
