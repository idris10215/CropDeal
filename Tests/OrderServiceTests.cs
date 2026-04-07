using Moq;
using NUnit.Framework;
using CropDeal.Models;
using CropDeal.Services;
using CropDeal.Repositories;

namespace CropDeal.Tests
{
    [TestFixture]
    public class OrderServiceTests
    {
        private Mock<IOrderRepository> _mockOrderRepo = null!;
        private Mock<ICropRepository> _mockCropRepo = null!;
        private OrderService _orderService = null!;

        [SetUp]
        public void Setup()
        {
            _mockOrderRepo = new Mock<IOrderRepository>();
            _mockCropRepo = new Mock<ICropRepository>();
            
            // Injecting Mocks into the real Service logic
            _orderService = new OrderService(_mockOrderRepo.Object, _mockCropRepo.Object);
        }

        [Test]
        public async Task PlaceOrderAsync_ShouldSetStatusToPending()
        {
            // Arrange
            var dto = new OrderCreateDto { CropId = 1, BuyerId = 2, Quantity = 10, TotalAmount = 100 };

            // Act
            var result = await _orderService.PlaceOrderAsync(dto);

            // Assert
            Assert.That(result, Is.EqualTo("Order placed! Awaiting farmer approval."));
            _mockOrderRepo.Verify(r => r.AddOrderAsync(It.Is<Order>(o => o.Status == "Pending")), Times.Once);
            _mockOrderRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task ApproveOrder_WhenStockIsEnough_ShouldUpdateStatusAndDeductStock()
        {
            var fakeCrop = new Crop { Id = 1, Quantity = 100 };
            var fakeOrder = new Order { Id = 10, CropId = 1, Quantity = 20, Status = "Pending" };

            _mockOrderRepo.Setup(r => r.GetOrderByIdAsync(10)).ReturnsAsync(fakeOrder);
            _mockCropRepo.Setup(r => r.GetCropByIdAsync(1)).ReturnsAsync(fakeCrop);

            // 2. Act
            var result = await _orderService.ApproveOrderAsync(10);

            Assert.That(fakeOrder.Status, Is.EqualTo("Approved"));
            Assert.That(fakeCrop.Quantity, Is.EqualTo(80)); // 100 - 20 = 80 
            Assert.That(result, Is.EqualTo("Order approved and stock updated successfully!"));
            
            _mockOrderRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
            _mockCropRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task ApproveOrder_WhenStockIsInsufficient_ShouldReturnErrorMessage()
        {
            var fakeCrop = new Crop { Id = 1, Quantity = 5 };
            var fakeOrder = new Order { Id = 10, CropId = 1, Quantity = 20, Status = "Pending" };

            _mockOrderRepo.Setup(r => r.GetOrderByIdAsync(10)).ReturnsAsync(fakeOrder);
            _mockCropRepo.Setup(r => r.GetCropByIdAsync(1)).ReturnsAsync(fakeCrop);

            // 2. Act
            var result = await _orderService.ApproveOrderAsync(10);

            Assert.That(result, Is.EqualTo("Insufficient stock to approve this order."));
            Assert.That(fakeOrder.Status, Is.EqualTo("Pending")); 
            Assert.That(fakeCrop.Quantity, Is.EqualTo(5)); 
        }
    }
}