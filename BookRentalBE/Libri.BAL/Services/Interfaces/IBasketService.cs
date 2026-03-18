using Libri.DAL.Models.Custom.CustomBasket;

namespace Libri.BAL.Services.Interfaces
{
    public interface IBasketService
    {
        Task<Basket?> GetBasketByKeyAsync(string id);
        Task<Basket?> SetBasketAsync(Basket basket);
        Task<bool> DeleteBasketByKeyAsync(string id);
    }
}