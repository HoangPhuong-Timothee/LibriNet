using Libri.DAL.DatabaseContext;
using Libri.DAL.Repositories;
using Libri.DAL.Repositories.Interfaces;

namespace Libri.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CustomContext _customContext;
        private readonly LibriContext _context;

        public UnitOfWork(LibriContext context, CustomContext customContext)
        {
            _customContext = customContext;
            _context = context;
            Dappers = new DapperRepository(_customContext);
            UserAddresses = new UserAddressRepository(_context);
            DeliveryMethods = new DeliveryMethodRepository(_context);
            BookStores = new BookStoreRepository(_context);
            UserAuths = new UserAuthRepository(_context);
            UserInfos = new UserInfoRepository(_context);
            Genres = new GenreRepository(_context);
            Publishers = new PublisherRepository(_context);
            Authors = new AuthorRepository(_context);
            Books = new BookRepository(_context);
            Orders = new OrderRepository(_context);
            UnitOfMeasures = new UnitOfMeasureRepository(_context);
        }

        public IDappperRepository Dappers { get; private set; }
        public IUserAddressRepository UserAddresses { get; private set; }
        public IDeliveryMethodRepository DeliveryMethods { get; private set; }
        public IBookStoreRepository BookStores { get; private set; }
        public IUserAuthRepository UserAuths { get; private set; }
        public IUserInfoRepository UserInfos { get; private set; }
        public IGenreRepository Genres { get; private set; }
        public IPublisherRepository Publishers { get; private set; }
        public IAuthorRepository Authors { get; private set; }
        public IBookRepository Books { get; private set; }
        public IOrderRepository Orders { get; private set; }
        public IUnitOfMeasureRepository UnitOfMeasures { get; private set; }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
