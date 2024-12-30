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
            // IN-MEMORY DATABASE
            var options = new DbContextOptionsBuilder<Dierentuin42Context>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new Dierentuin42Context(options);
            _controller = new CategoriesController(_context);

            // DATA SEEDEN
            _context.Category.Add(new Category { Id = 1, Name = "Mammals" });
            _context.Category.Add(new Category { Id = 2, Name = "Birds" });
            _context.SaveChanges();
        }

        [Fact]
        public async Task DeleteConfirmed_ShouldRedirectToIndex_WhenCategoryExists()
        {
            // ARRANGE
            var categoryId = 1;

            // ACT
            var result = await _controller.DeleteConfirmed(categoryId);

            // ASSERT
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }
    }
}
