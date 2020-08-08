using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore
{
    public class ProductController : Controller
    {
        private IProductRepository _repository;

        public ProductController(IProductRepository repository)
        {
            _repository = repository;
        }       

        public ViewResult List() => View(_repository.Products); 
    }
}
