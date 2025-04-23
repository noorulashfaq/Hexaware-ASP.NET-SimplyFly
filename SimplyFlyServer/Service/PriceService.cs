using SimplyFlyServer.Interface;
using SimplyFlyServer.Models.DTOs;
using SimplyFlyServer.Models;

namespace SimplyFlyServer.Service
{
    public class PriceService : IPriceService
    {
        private readonly IRepository<int, Price> _priceRepository;

        public PriceService(IRepository<int, Price> priceRepository)
        {
            _priceRepository = priceRepository;
        }

        public async Task<PriceResponse> AddPrice(PriceRequest priceRequest)
        {
            if (priceRequest == null)
                throw new ArgumentNullException(nameof(priceRequest));

            var price = MapToPrice(priceRequest);
            var result = await _priceRepository.Add(price);

            return MapToPriceResponse(result);
        }

        public async Task<PriceResponse> UpdatePrice(int id, PriceRequest priceRequest)
        {
            var existingPrice = await _priceRepository.GetById(id);
            if (existingPrice == null)
                throw new Exception("Price not found");

            existingPrice.FlightId = priceRequest.FlightId;
            existingPrice.PremiumClass = priceRequest.PremiumClass;
            existingPrice.EconomicClass = priceRequest.EconomyClass;
            existingPrice.BusinessClass = priceRequest.BusinessClass;

            var result = await _priceRepository.Update(id, existingPrice);
            return MapToPriceResponse(result);
        }

        public async Task<bool> DeletePrice(int id)
        {
            var price = await _priceRepository.GetById(id);
            if (price == null)
                return false;

            await _priceRepository.Delete(id);
            return true;
        }

        public async Task<PriceResponse> GetPriceById(int id)
        {
            var price = await _priceRepository.GetById(id);
            if (price == null)
                throw new Exception("Price not found");

            return MapToPriceResponse(price);
        }

        public async Task<IEnumerable<PriceResponse>> GetAllPrices()
        {
            var prices = await _priceRepository.GetAll();
            return prices.Select(p => MapToPriceResponse(p));
        }

        private Price MapToPrice(PriceRequest request)
        {
            return new Price
            {
                FlightId = request.FlightId,
                PremiumClass = request.PremiumClass,
                EconomicClass = request.EconomyClass,
                BusinessClass = request.BusinessClass
            };
        }

        private PriceResponse MapToPriceResponse(Price price)
        {
            return new PriceResponse
            {
                PriceId = price.PriceId,
                FlightId = price.FlightId,
                PremiumClass = price.PremiumClass,
                EconomyClass = price.EconomicClass,
                BusinessClass = price.BusinessClass
            };
        }
    }
}
