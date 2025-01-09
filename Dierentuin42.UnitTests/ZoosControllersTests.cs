using Xunit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Dierentuin42.Controllers;
using Dierentuin42.Data;
using Microsoft.EntityFrameworkCore;
using Dierentuin42.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dierentuin42.Tests
{
    public class ZoosControllerTests
    {
        private readonly ZoosController _controller;
        private readonly Mock<Dierentuin42Context> _mockContext;

        public ZoosControllerTests()
        {
            var options = new DbContextOptionsBuilder<Dierentuin42Context>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            _mockContext = new Mock<Dierentuin42Context>(options);
            _controller = new ZoosController(_mockContext.Object);
        }

        [Fact]
        public async void Create_Post_ReturnsRedirectToActionResult_WhenZooIsValid()
        {
            // ARRANGE
            var zoo = new Zoo { Name = "Zoo C" };
            _mockContext.Setup(m => m.Add(It.IsAny<Zoo>())).Verifiable();
            _mockContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // ACT
            var result = await _controller.Create(zoo, new List<int>());

            // ASSERT
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            _mockContext.Verify(m => m.Add(It.IsAny<Zoo>()), Times.Once);
        }

        
    }
}
