using Microsoft.EntityFrameworkCore.Storage;
using PRJ_MKS_BTT.Data;
using PRJ_MKS_BTT.IRepository;

namespace PRJ_MKS_BTT.Repository
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public EfUnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
