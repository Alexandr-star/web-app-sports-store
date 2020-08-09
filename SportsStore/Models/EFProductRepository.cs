using System.Linq;

namespace SportsStore.Models
{
    ///
    /// Класс хранилище
    /// Класс, который получает данные с ипользованеим EF Core
    ///
    public class EFProdutRepository : IProductRepository
    {
        private ApplicationDbContext _context;

        public EFProdutRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IQueryable<Product> Products => _context.Products;
    }
}
