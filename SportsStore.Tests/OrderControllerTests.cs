using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class OrderControllerTests
    {
        [Fact]
        public void Cannot_Checkout_Empty_Cart()
        {
        //Given
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            Cart cart = new Cart();
            Order order = new Order();
            OrderController target = new OrderController(mock.Object, cart);
        //When
            ViewResult result = target.Checkout(order) as ViewResult;
        //Then
        // Заказ не был сохранен
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
        // Возвращает стандартное представление
            Assert.True(string.IsNullOrEmpty(result.ViewName));
        // Представлению передана недопустимая модель
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
        //Given
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            Order order = new Order();
            OrderController target = new OrderController(mock.Object, cart);
            target.ModelState.AddModelError("error", "error");
        //When
            ViewResult result = target.Checkout(order) as ViewResult;

        //Then
        // Заказ не был сохранен
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
        // Возвращает стандартное представление
            Assert.True(string.IsNullOrEmpty(result.ViewName));
        // Представлению передана недопустимая модель
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Can_Checkout_And_Submit_Order()
        {
        //Given
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            Order order = new Order();
            OrderController target = new OrderController(mock.Object, cart);
        //When
            RedirectToActionResult result = target.Checkout(order) as RedirectToActionResult;
        //Then
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);
            Assert.Equal("Completed", result.ActionName);
        }
    }
}