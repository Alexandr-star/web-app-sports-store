using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using SportsStore.Components;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class NavigationMenuViewComponentTest
    {
        [Fact]
        public void Can_Select_Categories()
        {
            //Given
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product { ProductId = 1, Name = "p1", Category = "text" },
                new Product { ProductId = 2, Name = "p2", Category = "text" },
                new Product { ProductId = 3, Name = "p3", Category = "box" },
                new Product { ProductId = 4, Name = "p4", Category = "pants" },
            }).AsQueryable<Product>());
            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);
            
            //When
            string[] result = ((IEnumerable<string>)(target.Invoke() as ViewViewComponentResult)
                .ViewData.Model).ToArray();

            //Then
            Assert.True(Enumerable.SequenceEqual(new string[] {
                "box",
                "pants",
                "text" 
            }, result));
        }

        [Fact]
        public void Indicates_Selected_Category()
        {
            //Given
            string categoryToSelect = "text";
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product { ProductId = 1, Name = "p1", Category = "text" },
                new Product { ProductId = 4, Name = "p4", Category = "pants" },
            }).AsQueryable<Product>());
            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);
            target.ViewComponentContext = new ViewComponentContext {
                ViewContext = new ViewContext {
                    RouteData = new RouteData()
                }
            };
            target.RouteData.Values["category"] = categoryToSelect;
            //When
            string result = (string)(target.Invoke() as ViewViewComponentResult).ViewData["SelectedCategory"];
            //Then
            Assert.Equal(categoryToSelect, result);
        }
    }
}