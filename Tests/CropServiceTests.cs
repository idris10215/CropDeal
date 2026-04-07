using Moq;
using NUnit.Framework;
using CropDeal.Models;
using CropDeal.Services;
using CropDeal.Repositories;

namespace CropDeal.Tests
{
    [TestFixture]
    public class CropServiceTests
    {
        private Mock<ICropRepository> _mockCropRepo = null!;
        private CropService _cropService = null!;

        [SetUp]
        public void Setup()
        {
            _mockCropRepo = new Mock<ICropRepository>();
            _cropService = new CropService(_mockCropRepo.Object);
        }

        [Test]
        public async Task AddCropAsync_ShouldCallRepositoryAddAndSave()
        {
            // 1. Arrange: Prepare a valid Crop DTO
            var dto = new CropCreateDto 
            { 
                Name = "Basmati Rice", 
                Category = "Grain", 
                Price = 60.0m, 
                Quantity = 50, 
                SellerId = 1 
            };

            // 2. Act
            await _cropService.AddCropAsync(dto);

            // 3. Assert: Verify the repository was called correctly
            _mockCropRepo.Verify(r => r.AddCropAsync(It.IsAny<Crop>()), Times.Once);
            _mockCropRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteCropAsync_WhenCropExists_ShouldReturnTrue()
        {
            // 1. Arrange: Mock an existing crop in the DB
            var fakeCrop = new Crop { Id = 1, Name = "Wheat" };
            _mockCropRepo.Setup(r => r.GetCropByIdAsync(1)).ReturnsAsync(fakeCrop);

            // 2. Act
            var result = await _cropService.DeleteCropAsync(1);

            // 3. Assert
            Assert.IsTrue(result);
            _mockCropRepo.Verify(r => r.DeleteCrop(fakeCrop), Times.Once);
            _mockCropRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateCropAsync_WhenCropNotFound_ShouldReturnFalse()
        {
            // 1. Arrange: Setup mock to return null (crop doesn't exist)
            _mockCropRepo.Setup(r => r.GetCropByIdAsync(999)).ReturnsAsync((Crop?)null);

            // 2. Act
            var result = await _cropService.UpdateCropAsync(999, new CropCreateDto());

            // 3. Assert
            Assert.IsFalse(result);
        }
    }
}