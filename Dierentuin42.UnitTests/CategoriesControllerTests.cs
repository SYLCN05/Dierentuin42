using Microsoft.AspNetCore.Mvc;
using Dierentuin42.Controllers;
using Dierentuin42.Models;
using Dierentuin42.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Dierentuin42.Tests
{
    public class CategoriesControllerTests
    {
        private readonly Dierentuin42Context _context;
        private readonly CategoriesController _controller;

        public CategoriesControllerTests()
        {
            // Set up an in-memory database
            var options = new DbContextOptionsBuilder<Dierentuin42Context>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new Dierentuin42Context(options);
            _controller = new CategoriesController(_context);

            // Add some test data
            _context.Category.Add(new Category { Id = 1, Name = "Mammals" });
            _context.Category.Add(new Category { Id = 2, Name = "Birds" });
            _context.SaveChanges();
        }

        [Fact]
        public async Task DeleteConfirmed_ShouldRedirectToIndex_WhenCategoryExists()
        {
            // Arrange
            var categoryId = 1;

            // Act
            var result = await _controller.DeleteConfirmed(categoryId);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }
    }
}
