using Libri.DAL.Repositories.Interfaces;

namespace Libri.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IDappperRepository Dappers { get; }
        IUserAddressRepository UserAddresses { get; }
        IDeliveryMethodRepository DeliveryMethods { get; }
        IBookStoreRepository BookStores { get; }
        IUserAuthRepository UserAuths { get; }
        IUserInfoRepository UserInfos { get; }
        IGenreRepository Genres { get; }
        IPublisherRepository Publishers { get; }
        IAuthorRepository Authors { get; }
        IBookRepository Books { get; }
        IOrderRepository Orders { get; }
        IUnitOfMeasureRepository UnitOfMeasures { get; }
        Task<int> CommitAsync();
    }
}
