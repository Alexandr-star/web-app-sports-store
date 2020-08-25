using System.Linq;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class CartTests
    {

        [Fact]
        public void Can_Add_New_Lines()
        {
            //Given
            Product p1 = new Product { ProductId = 1, Name = "p1" };
            Product p2 = new Product { ProductId = 2, Name = "p2" };

            Cart target = new Cart();
            //When
            
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            CartLine[] result = target.Lines.ToArray();
            
            //Then
            Assert.Equal(2, result.Length);
            Assert.Equal(p1, result[0].Product);
            Assert.Equal(p2, result[1].Product);
        }

        [Fact]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            //Given
            Product p1 = new Product { ProductId = 1, Name = "p1" };
            Product p2 = new Product { ProductId = 2, Name = "p2" };
            Cart target = new Cart();
            //When
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 10);
            CartLine[] result = target.Lines.OrderBy(c => c.Product.ProductId).ToArray();
            //Then
            Assert.Equal(2, result.Length);
            Assert.Equal(11, result[0].Quality);
            Assert.Equal(1, result[1].Quality);
        }

        [Fact]
        public void Can_Remove_Line()
        {
            //Given
            Product p1 = new Product { ProductId = 1, Name = "p1" };
            Product p2 = new Product { ProductId = 2, Name = "p2" };
            Product p3 = new Product { ProductId = 3, Name = "p3" };
            Cart target = new Cart();
            //When
            target.AddItem(p1, 1);
            target.AddItem(p2, 3);
            target.AddItem(p3, 5);
            target.AddItem(p2, 1);
            target.RemoveLine(p2);
            //Then
            Assert.Equal(0, target.Lines.Where(c => c.Product == p2).Count());
            Assert.Equal(2, target.Lines.Count());

        }

        [Fact]
        public void Calculate_Cart_total()
        {
            //Given
            Product p1 = new Product { ProductId = 1, Name = "p1", Price= 100M };
            Product p2 = new Product { ProductId = 2, Name = "p2", Price = 50M };
            Cart target = new Cart();
            //When
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 3);
            decimal result = target.ComputeTotalValue();
            //Then
            Assert.Equal(450M, result);
        }

        [Fact]
        public void Can_Clear_Contents()
        {
            //Given
            Product p1 = new Product { ProductId = 1, Name = "p1", Price= 100M };
            Product p2 = new Product { ProductId = 2, Name = "p2", Price = 50M };
            Cart target = new Cart();
            //When
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.Clear();
            //Then
            Assert.Equal(0, target.Lines.Count());
        }
    }
}