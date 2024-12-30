using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dierentuin42.Controllers;
using Dierentuin42.Models;
using Dierentuin42.Data;

namespace Dierentuin42.Tests
{
    public class AnimalsControllerTests
    {
        private Dierentuin42Context CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<Dierentuin42Context>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            return new Dierentuin42Context(options);
        }

        [Fact]
        public async Task Create_Post_AddsAnimal()
        {
            // ARRANGE
            var context = CreateDbContext();
            var controller = new AnimalsController(context);

            var newAnimal = new Animal
            {
                Name = "Lion",
                Species = "Panthera leo",
                CategoryId = 1,
                AnimalSize = Animal.Size.Large,
                AnimalDiet = Animal.DietaryClass.Carnivore,
                AnimalActivityPattern = Animal.ActivityPattern.Diurnal,
                Prey = "Deer",
                EnclosureId = 1,
                SecurityRequirement = Animal.SecurityLevel.High,
                spaceRequirement = 20.5
            };

            // ACT
            var result = await controller.Create(newAnimal);

            // ASSERT
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            var createdAnimal = await context.Animal.FindAsync(newAnimal.Id);
            Assert.NotNull(createdAnimal);
            Assert.Equal("Lion", createdAnimal.Name);
        }

        [Fact]
        public async Task Delete_Post_RemovesAnimal()
        {
            // ARRANGE
            var context = CreateDbContext();
            var controller = new AnimalsController(context);

            var animal = new Animal
            {
                Name = "Lion",
                Species = "Panthera leo",
                CategoryId = 1,
                AnimalSize = Animal.Size.Large,
                AnimalDiet = Animal.DietaryClass.Carnivore,
                AnimalActivityPattern = Animal.ActivityPattern.Diurnal,
                Prey = "Deer",
                EnclosureId = 1,
                SecurityRequirement = Animal.SecurityLevel.High,
                spaceRequirement = 20.5
            };
            context.Animal.Add(animal);
            await context.SaveChangesAsync();

            // ACT
            var result = await controller.DeleteConfirmed(animal.Id);

            // ASSERT
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            var deletedAnimal = await context.Animal.FindAsync(animal.Id);
            Assert.Null(deletedAnimal); 
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithAnimal()
        {
            // ARRANGE
            var context = CreateDbContext();
            var controller = new AnimalsController(context);

            var animal = new Animal
            {
                Name = "Lion",
                Species = "Panthera leo",
                CategoryId = 1,
                AnimalSize = Animal.Size.Large,
                AnimalDiet = Animal.DietaryClass.Carnivore,
                AnimalActivityPattern = Animal.ActivityPattern.Diurnal,
                Prey = "Deer",
                EnclosureId = 1,
                SecurityRequirement = Animal.SecurityLevel.High,
                spaceRequirement = 20.5
            };
            context.Animal.Add(animal);
            await context.SaveChangesAsync();

            // ACT
            var result = await controller.Details(animal.Id);

            // ASSERT
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Animal>(viewResult.ViewData.Model);
            Assert.Equal(animal.Id, model.Id);
            Assert.Equal("Lion", model.Name);
        }

        [Fact]
        public async Task Edit_Get_ReturnsNotFound_WhenAnimalDoesNotExist()
        {
            // ARRANGE
            var context = CreateDbContext();
            var controller = new AnimalsController(context);

            // ACT
            var result = await controller.Edit(9999);  

            // ASSERT
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_Get_ReturnsNotFound_WhenAnimalDoesNotExist()
        {
            // ARRANGE
            var context = CreateDbContext();
            var controller = new AnimalsController(context);

            // ACT
            var result = await controller.Delete(9999);  

            // ASSERT
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
