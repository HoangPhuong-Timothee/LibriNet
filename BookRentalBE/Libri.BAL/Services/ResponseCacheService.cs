using System.Text.Json;
using Libri.BAL.Services.Interfaces;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Libri.BAL.Services;

public class ResponseCacheService : IResponseCacheService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _database;
    private readonly ILogger<ResponseCacheService> _logger;
    public ResponseCacheService(IConnectionMultiplexer redis, ILogger<ResponseCacheService> logger)
    {
        _logger = logger;
        _redis = redis;
        _database = _redis.GetDatabase();
    }

    public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan expiration)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var serializedResponse = JsonSerializer.Serialize(response, options);
            await _database.StringSetAsync(cacheKey, serializedResponse, expiration);
        }
        catch (Exception e)
        {
            _logger.LogError("Lỗi lưu dữ liệu vào cache: {Exception}", e);

            if (e.InnerException != null)
            {
                _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
            }

            throw new Exception("Có lỗi xảy ra trong quá trình lưu dữ liệu vào cache.");
        }
    }

    public async Task<string?> GetCachedResponseAsync(string cacheKey)
    {
        try
        {
            var cacheResponse = await _database.StringGetAsync(cacheKey);

            if (cacheResponse.IsNullOrEmpty)
            {
                return null;
            }

            return cacheResponse;
        }
        catch (Exception e)
        {
            _logger.LogError("Lỗi lấy dữ liệu từ cache: {Exception}", e);

            if (e.InnerException != null)
            {
                _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
            }

            throw new Exception("Có lỗi xảy ra trong quá trình lấy dữ liệu từ cache.");
        }
    }

    public async Task RemoveCacheByPattern(string pattern)
    {
        try
        {
            var server = _redis.GetServer(_redis.GetEndPoints().First());
            var keys = server.Keys(database: _database.Database, pattern: $"*{pattern}*").ToArray();

            if (keys.Length != 0)
            {
                await _database.KeyDeleteAsync(keys);
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Lỗi xóa dữ liệu khỏi cache: {Exception}", e);

            if (e.InnerException != null)
            {
                _logger.LogError("Chi tiết lỗi: {InnerException}", e.InnerException.Message);
            }

            throw new Exception("Có lỗi xảy ra trong quá trình xóa dữ liệu khỏi cache.");
        }
    }
}