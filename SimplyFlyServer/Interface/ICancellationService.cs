using SimplyFlyServer.Models.DTOs;

namespace SimplyFlyServer.Interface
{
    public interface ICancellationService
    {
        Task<CancellationResponse> CancelBooking(CancellationRequest request);
        Task<IEnumerable<CancellationResponse>> GetAllCancellations();
        Task<CancellationResponse> GetCancellationById(int id);
        public  Task<CancellationResponse> UpdateRefundStatus(int cancelId, string newStatus);
    }
}
