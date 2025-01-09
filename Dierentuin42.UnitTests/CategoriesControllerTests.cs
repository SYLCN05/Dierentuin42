using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dierentuin42.Controllers;
using Dierentuin42.Models;
using Dierentuin42.Data;

namespace Dierentuin42.Tests
{
    public class CategoriesControllerTests
    {
        private Dierentuin42Context CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<Dierentuin42Context>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            return new Dierentuin42Context(options);
        }

        [Fact]
        public async Task Create_Post_AddsCategory()
        {
            // ARRANGE
            var context = CreateDbContext();
            var controller = new CategoriesController(context);

            var newCategory = new Category
            {
                Name = "New Category"
            };

            // ACT
            var result = await controller.Create(newCategory, new List<int>());  

            // ASSERT
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            var createdCategory = await context.Category.FindAsync(newCategory.Id);
            Assert.NotNull(createdCategory);
            Assert.Equal("New Category", createdCategory.Name);
        }

        [Fact]
        public async Task Edit_Get_ReturnsViewResult_WithCategory()
        {
            // ARRANGE
            var context = CreateDbContext();
            var controller = new CategoriesController(context);

            var category = new Category
            {
                Name = "Original Category"
            };
            context.Category.Add(category);
            await context.SaveChangesAsync();

            // ACT
            var result = await controller.Edit(category.Id);

            // ASSERT
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Category>(viewResult.ViewData.Model);
            Assert.Equal(category.Id, model.Id);
            Assert.Equal("Original Category", model.Name);
        }

        [Fact]
        public async Task Edit_Post_UpdatesCategory()
        {
            // ARRANGE
            var context = CreateDbContext();
            var controller = new CategoriesController(context);

            var category = new Category
            {
                Name = "Original Category"
            };
            context.Category.Add(category);
            await context.SaveChangesAsync();

            var updatedCategory = new Category
            {
                Id = category.Id,  
                Name = "Updated Category"
            };

            // ACT
            var result = await controller.Edit(updatedCategory.Id, updatedCategory, new List<int>());  

            // ASSERT
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            var updatedCategoryFromDb = await context.Category.FindAsync(updatedCategory.Id);
            Assert.NotNull(updatedCategoryFromDb);
            Assert.Equal("Updated Category", updatedCategoryFromDb.Name);
        }

        [Fact]
        public async Task Delete_Get_ReturnsViewResult_WithCategory()
        {
            // ARRANGE
            var context = CreateDbContext();
            var controller = new CategoriesController(context);

            var category = new Category
            {
                Name = "Delete Me"
            };
            context.Category.Add(category);
            await context.SaveChangesAsync();

            // ACT
            var result = await controller.Delete(category.Id);

            // ASSERT
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Category>(viewResult.ViewData.Model);
            Assert.Equal(category.Id, model.Id);
        }

        [Fact]
        public async Task Delete_Post_RemovesCategory()
        {
            // ARRANGE
            var context = CreateDbContext();
            var controller = new CategoriesController(context);

            var category = new Category
            {
                Name = "Delete Me"
            };
            context.Category.Add(category);
            await context.SaveChangesAsync();

            // ACT
            var result = await controller.DeleteConfirmed(category.Id);

            // ASSERT
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            var deletedCategory = await context.Category.FindAsync(category.Id);
            Assert.Null(deletedCategory);  
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithCategory()
        {
            // ARRANGE
            var context = CreateDbContext();
            var controller = new CategoriesController(context);

            var category = new Category
            {
                Name = "Test Category"
            };
            context.Category.Add(category);
            await context.SaveChangesAsync();

            // ACT
            var result = await controller.Details(category.Id);

            // ASSERT
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Category>(viewResult.ViewData.Model);
            Assert.Equal(category.Id, model.Id);
            Assert.Equal("Test Category", model.Name);
        }

        [Fact]
        public async Task Edit_Get_ReturnsNotFound_WhenCategoryDoesNotExist()
        {
            // ARRANGE
            var context = CreateDbContext();
            var controller = new CategoriesController(context);

            // ACT
            var result = await controller.Edit(9999); 

            // ASSERT
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_Get_ReturnsNotFound_WhenCategoryDoesNotExist()
        {
            // ARRANGE
            var context = CreateDbContext();
            var controller = new CategoriesController(context);

            // ACT
            var result = await controller.Delete(9999); 

            // ASSERT
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
