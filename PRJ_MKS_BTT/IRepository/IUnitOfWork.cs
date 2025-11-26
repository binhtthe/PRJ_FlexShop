using Microsoft.EntityFrameworkCore.Storage;

namespace PRJ_MKS_BTT.IRepository
{
    public interface IUnitOfWork
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<int> SaveChangesAsync();
    }
}
