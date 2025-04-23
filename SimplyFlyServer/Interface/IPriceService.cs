using SimplyFlyServer.Models;
using SimplyFlyServer.Models.DTOs;

namespace SimplyFlyServer.Interface
{
    public interface IPriceService
    {
        Task<IEnumerable<PriceResponse>> GetAllPrices();
        Task<PriceResponse> GetPriceById(int id);
        Task<PriceResponse> AddPrice(PriceRequest request);
        Task<PriceResponse> UpdatePrice(int id, PriceRequest request);
        Task<bool> DeletePrice(int id);
    }
}
