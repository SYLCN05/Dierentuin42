using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dierentuin42.Controllers;
using Dierentuin42.Models;
using Dierentuin42.Data;

namespace Dierentuin42.Tests
{
    public class EnclosuresControllerTests
    {
        private Dierentuin42Context CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<Dierentuin42Context>()
                .UseInMemoryDatabase(databaseName: "TestDatabase") 
                .Options;

            return new Dierentuin42Context(options);
        }


        [Fact]
        public async Task Create_Post_AddsEnclosure()
        {
            // TEST OF DE CREATE ACTIE EEN NIEUW OBJECT TOEVOEGT EN DOORVERWIJST NAAR DE METHODE.
            // ARRANGE
            var context = CreateDbContext();
            var controller = new EnclosuresController(context);

            var newEnclosure = new Enclosure
            {
                Name = "New Enclosure",
                EnclosureClimate = Enclosure.Climate.Temperate,
                EnclosureHabitatType = Enclosure.HabitatType.Aquatic,
                EnclosureSecurityLevel = Enclosure.SecurityLevel.Low,
                Size = 250.75
            };

            // ACT
            var result = await controller.Create(newEnclosure, new List<int>());

            // ASSERT
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            var createdEnclosure = await context.Enclosure.FindAsync(newEnclosure.Id);
            Assert.NotNull(createdEnclosure);
            Assert.Equal("New Enclosure", createdEnclosure.Name);
        }

        [Fact]
        public async Task Edit_Get_ReturnsViewResult_WithEnclosure()
        {
            // TEST OF DE EDIT GET ACTIE DE GEGEVENS VAN EEN SPECIFIEK OBJECT LAADT.

            // ARRANGE
            var context = CreateDbContext();
            var controller = new EnclosuresController(context);

            var enclosure = new Enclosure
            {
                Name = "Original Enclosure",
                EnclosureClimate = Enclosure.Climate.Temperate,
                EnclosureHabitatType = Enclosure.HabitatType.Desert,
                EnclosureSecurityLevel = Enclosure.SecurityLevel.High,
                Size = 400.25
            };
            context.Enclosure.Add(enclosure);
            await context.SaveChangesAsync();

            // ACT
            var result = await controller.Edit(enclosure.Id);

            // ASSERT
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Enclosure>(viewResult.ViewData.Model);
            Assert.Equal(enclosure.Id, model.Id);
            Assert.Equal("Original Enclosure", model.Name);
        }

        [Fact]
        public async Task Edit_Post_UpdatesEnclosure()
        {
            // TEST OF DE EDIT POST ACTIE EEN OBJECT CORRECT UPDATET IN DE DATABASE.

            // ARRANGE
            var context = CreateDbContext();
            var controller = new EnclosuresController(context);

            var enclosure = new Enclosure
            {
                Name = "Original Enclosure",
                EnclosureClimate = Enclosure.Climate.Temperate,
                EnclosureHabitatType = Enclosure.HabitatType.Desert,
                EnclosureSecurityLevel = Enclosure.SecurityLevel.High,
                Size = 400.25
            };
            context.Enclosure.Add(enclosure);
            await context.SaveChangesAsync();

            var updatedEnclosure = new Enclosure
            {
                Id = enclosure.Id,  
                Name = "Updated Enclosure",
                EnclosureClimate = Enclosure.Climate.Tropical,
                EnclosureHabitatType = Enclosure.HabitatType.Forest,
                EnclosureSecurityLevel = Enclosure.SecurityLevel.Medium,
                Size = 500.75
            };

            // ACT
            var result = await controller.Edit(updatedEnclosure.Id, updatedEnclosure, new List<int>());

            // ASSERT
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            var createdEnclosure = await context.Enclosure.FindAsync(updatedEnclosure.Id);
            Assert.NotNull(createdEnclosure);
            Assert.Equal("Updated Enclosure", createdEnclosure.Name);
            Assert.Equal(Enclosure.Climate.Tropical, createdEnclosure.EnclosureClimate);
            Assert.Equal(Enclosure.HabitatType.Forest, createdEnclosure.EnclosureHabitatType);
        }

        [Fact]
        public async Task Delete_Get_ReturnsViewResult_WithEnclosure()
        {
            // TEST OF DE DELETE GET ACTIE DE GEGEVENS VAN EEN TE VERWIJDEREN OBJECT LAADT.
            // ARRANGE
            var context = CreateDbContext();
            var controller = new EnclosuresController(context);

            var enclosure = new Enclosure
            {
                Name = "Delete Me",
                EnclosureClimate = Enclosure.Climate.Temperate,
                EnclosureHabitatType = Enclosure.HabitatType.Grassland,
                EnclosureSecurityLevel = Enclosure.SecurityLevel.Low,
                Size = 300.5
            };
            context.Enclosure.Add(enclosure);
            await context.SaveChangesAsync();

            // ACT
            var result = await controller.Delete(enclosure.Id);

            // ASSERT
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Enclosure>(viewResult.ViewData.Model);
            Assert.Equal(enclosure.Id, model.Id);
        }

        [Fact]
        public async Task Delete_Post_RemovesEnclosure()
        {
            // TEST OF DE DELETE POST ACTIE EEN OBJECT WEGHAALD UIT DE DATABASE.
            // ARRANGE
            var context = CreateDbContext();
            var controller = new EnclosuresController(context);

            var enclosure = new Enclosure
            {
                Name = "Delete Me",
                EnclosureClimate = Enclosure.Climate.Temperate,
                EnclosureHabitatType = Enclosure.HabitatType.Grassland,
                EnclosureSecurityLevel = Enclosure.SecurityLevel.Low,
                Size = 300.5
            };
            context.Enclosure.Add(enclosure);
            await context.SaveChangesAsync();

            // ACT
            var result = await controller.DeleteConfirmed(enclosure.Id);

            // ASSERT
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            var deletedEnclosure = await context.Enclosure.FindAsync(enclosure.Id);
            Assert.Null(deletedEnclosure); 
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithEnclosure()
        {
            // TEST OF DE DETAILS ACTIE DE GEGEVENS VAN EEN OBJECT TOONT.
            // ARRANGE
            var context = CreateDbContext();
            var controller = new EnclosuresController(context);

            var enclosure = new Enclosure
            {
                Name = "Test Enclosure",
                EnclosureClimate = Enclosure.Climate.Tropical,
                EnclosureHabitatType = Enclosure.HabitatType.Forest,
                EnclosureSecurityLevel = Enclosure.SecurityLevel.Medium,
                Size = 500.5
            };
            context.Enclosure.Add(enclosure);
            await context.SaveChangesAsync();

            // ACT
            var result = await controller.Details(enclosure.Id);

            // ASSERT
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Enclosure>(viewResult.ViewData.Model);
            Assert.Equal(enclosure.Id, model.Id);
            Assert.Equal("Test Enclosure", model.Name);
        }

        [Fact]
        public async Task Edit_Get_ReturnsNotFound_WhenEnclosureDoesNotExist()
        {
            // TEST OF DE EDIT GET ACTIE EEN FOUT MEEGEVENWAT GEEN OBJECT BESTAAT.
            // ARRANGE
            var context = CreateDbContext();
            var controller = new EnclosuresController(context);

            // ACT
            var result = await controller.Edit(9999); 

            // ASSERT
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_Get_ReturnsNotFound_WhenEnclosureDoesNotExist()
        {
            // TEST OF DE DELETE GET ACTIE EEN FOUT MEEGEVENWAT GEEN OBJECT BESTAAT.
            // ARRANGE
            var context = CreateDbContext();
            var controller = new EnclosuresController(context);

            // ACT
            var result = await controller.Delete(9999);  

            // ASSERT
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
