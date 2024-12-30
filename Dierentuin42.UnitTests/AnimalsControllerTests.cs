using Moq;
using Dierentuin42.Controllers;
using Dierentuin42.Models;
using Dierentuin42.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Dierentuin42.Tests
{
    public class AnimalsControllerTests
    {
        private readonly AnimalsController _controller;
        private readonly Dierentuin42Context _context;

        public AnimalsControllerTests()
        {
            // IN-MEMORY DATABASE
            var options = new DbContextOptionsBuilder<Dierentuin42Context>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new Dierentuin42Context(options);  
            _controller = new AnimalsController(_context);
        }

        [Fact]
        public async Task Details_ShouldReturnNotFound_WhenAnimalNotFound()
        {
            // ARRANGE
            var animalId = 999;

            // ACT
            var result = await _controller.Details(animalId);

            // ASSERT
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ShouldReturnNotFound_WhenAnimalDoesNotExist()
        {
            // ARRANGE
            var animalId = 999;
            var animal = new Animal { Id = animalId, Name = "Zebra", Species = "Equus zebra" };

            // ACT
            var result = await _controller.Edit(animalId, animal);

            // ASSERT
            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenAnimalDoesNotExist()
        {
            // ARRANGE
            var animalId = 999;

            // ACT
            var result = await _controller.Delete(animalId);

            // ASSERT
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
