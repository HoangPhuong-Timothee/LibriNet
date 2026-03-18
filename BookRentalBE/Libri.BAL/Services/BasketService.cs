using System.Text.Json;
using Libri.BAL.Services.Interfaces;
using Libri.DAL.Models.Custom.CustomBasket;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Libri.BAL.Services
{
    public class BasketService : IBasketService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _database;
        private readonly ILogger<BasketService> _logger;
        public BasketService(IConnectionMultiplexer redis, ILogger<BasketService> logger)
        {
            _logger = logger;
            _redis = redis;
            _database = _redis.GetDatabase();
        }
        public async Task<Basket?> GetBasketByKeyAsync(string id)
        {
            try
            {
                var data = await _database.StringGetAsync(id);

                return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<Basket>(data!);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new Exception("Có lỗi xảy ra khi lấy thông tin giỏ sách.");
            }
        }

        public async Task<Basket?> SetBasketAsync(Basket basket)
        {
            try
            {
                var setBasket = await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));
                
                if (!setBasket)
                {
                    _logger.LogError("Không thể cập nhật giỏ sách mã '{basketKey}'.", basket.Id);
                    return null;
                }

                return await GetBasketByKeyAsync(basket.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new Exception("Có lỗi xảy ra khi cập nhật giỏ sách.");
            }
        }
        public async Task<bool> DeleteBasketByKeyAsync(string id)
        {
            try
            {
                return await _database.KeyDeleteAsync(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new Exception("Có lỗi xảy ra khi xóa giỏ sách.");
            }
        }
    }
}