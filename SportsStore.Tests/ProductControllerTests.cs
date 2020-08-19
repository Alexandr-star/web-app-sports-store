using System.Collections.Generic;
using System.Linq;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using Xunit;

namespace SportsSpre.Test
{
    public class ProductControllerTests
    {
        [Fact]
        public void Can_Paginate()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product { ProductId = 1, Name = "P1" },
                new Product { ProductId = 2, Name = "P2" },
                new Product { ProductId = 3, Name = "P3" },
                new Product { ProductId = 4, Name = "P4" },
                new Product { ProductId = 5, Name = "P5" }
            }).AsQueryable<Product>());

            ProductController controller = new ProductController(mock.Object)   { PageSize = 3 };

            ProductsListViewModel result = controller.List(null, 2).ViewData.Model as ProductsListViewModel;

            PagingInfo pagingInfo = result.PagingInfo;
            Assert.Equal(2, pagingInfo.CurrentPage);
            Assert.Equal(3, pagingInfo.ItemsPerPage);
            Assert.Equal(5, pagingInfo.TotalItems);
            Assert.Equal(2, pagingInfo.TotalPages);
        }

        [Fact]
        public void Can_Filter_Products()
        {
            //  Создание имитированного хранилища
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product { ProductId = 1, Name = "P1", Category = "Game1"},
                new Product { ProductId = 2, Name = "P2", Category = "Game3"},
                new Product { ProductId = 3, Name = "P3", Category = "Game2"},
                new Product { ProductId = 4, Name = "P4", Category = "Game2"},
                new Product { ProductId = 5, Name = "P5", Category = "Game3"},
                new Product { ProductId = 6, Name = "P6", Category = "Game1"},
                new Product { ProductId = 7, Name = "P7", Category = "Game2"},
                new Product { ProductId = 8, Name = "P8", Category = "Game1"},
            }).AsQueryable<Product>());

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            Product[] result = (controller.List("Game2", 1).ViewData.Model as ProductsListViewModel)
                .Products.ToArray();

            Assert.Equal(3, result.Length);
            Assert.True(result[0].Name == "P3" && result[0].Category == "Game2");
            Assert.True(result[1].Name == "P4" && result[1].Category == "Game2");
            Assert.True(result[2].Name == "P7" && result[2].Category == "Game2");
        }
    }
}