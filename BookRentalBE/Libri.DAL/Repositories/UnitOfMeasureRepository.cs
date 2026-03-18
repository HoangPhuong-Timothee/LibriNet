using Libri.DAL.DatabaseContext;
using Libri.DAL.Models.Domain;
using Libri.DAL.Repositories.Interfaces;

namespace Libri.DAL.Repositories
{
    public class UnitOfMeasureRepository : GenericRepository<UnitOfMeasure>, IUnitOfMeasureRepository
    {
        public UnitOfMeasureRepository(LibriContext context) : base(context)
        {

        }
    }
}
