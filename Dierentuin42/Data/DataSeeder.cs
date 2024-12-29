using Bogus;
using Dierentuin42.Models;
using Dierentuin42.Data;
using System;
using System.Linq;

namespace Dierentuin42.Data
{
    public static class DataSeeder
    {
        public static void Seed(IServiceProvider serviceProvider, Dierentuin42Context context)
        {

            // ALLES VERWIJDEREN BIJ RUN
            context.Animal.RemoveRange(context.Animal);
            context.Enclosure.RemoveRange(context.Enclosure);
            context.Zoo.RemoveRange(context.Zoo);
            context.Category.RemoveRange(context.Category);

            context.SaveChanges();

            //CHECK OF DATABASE IS GESEED
            if (context.Category.Any())
                return;

            // CATEGORIES SEEDEN
            var categoryFaker = new Faker<Category>()
                .RuleFor(c => c.Name, f => f.Commerce.Department())
                .Generate(5);

            context.Category.AddRange(categoryFaker);
            context.SaveChanges();

            // ZOOS SEEDEN
            var zooFaker = new Faker<Zoo>()
                .RuleFor(z => z.Name, f => f.Company.CompanyName())
                .Generate(3);

            context.Zoo.AddRange(zooFaker);
            context.SaveChanges();

            // ENCLOSURES SEEDEN
            var enclosureFaker = new Faker<Enclosure>()
                .RuleFor(e => e.Name, f => f.Commerce.ProductName())
                .RuleFor(e => e.EnclosureClimate, f => f.PickRandom<Enclosure.Climate>())
                .RuleFor(e => e.EnclosureHabitatType, f => f.PickRandom<Enclosure.HabitatType>())
                .RuleFor(e => e.EnclosureSecurityLevel, f => f.PickRandom<Enclosure.SecurityLevel>())
                .RuleFor(e => e.Size, f => Math.Round(f.Random.Double(5000, 10000),2))
                .RuleFor(e => e.ZooId, f => f.PickRandom(context.Zoo.Select(z => z.Id).ToList()))
                .Generate(5);

            context.Enclosure.AddRange(enclosureFaker);
            context.SaveChanges();

            // LIJST MET MOGELIJKE DIERSOORTEN, IK HEB DIT HANDMATIG GEDAAN OMDAT BOGUS GEEN COLLECTIE HEEFT DIE ENIGSZINS OP DEZE CATEGORIE LIJKT
            var speciesList = new[] { "Leeuw", "Tijger", "Olifant", "Giraf", "Zebra", "Neushoorn", "Aap", "Krokodil", "Panda", "Kangoeroe" };

            // ANIMALS SEEDEN
            var animalFaker = new Faker<Animal>()
                .RuleFor(a => a.Name, f => f.Name.FirstName())
                .RuleFor(a => a.Species, f => f.PickRandom(speciesList))
                .RuleFor(a => a.AnimalSize, f => f.PickRandom<Animal.Size>())
                .RuleFor(a => a.AnimalDiet, f => f.PickRandom<Animal.DietaryClass>())
                .RuleFor(a => a.AnimalActivityPattern, f => f.PickRandom<Animal.ActivityPattern>())
                .RuleFor(a => a.Prey, f => f.Commerce.Product())
                .RuleFor(a => a.CategoryId, f => f.PickRandom(context.Category.Select(c => c.Id).ToList()))
                .RuleFor(a => a.EnclosureId, f => f.PickRandom(context.Enclosure.Select(e => e.Id).ToList()))
                .RuleFor(a => a.SecurityRequirement, f => f.PickRandom<Animal.SecurityLevel>())
                .RuleFor(a => a.spaceRequirement, f => Math.Round(f.Random.Double(1, 500), 2))

                .Generate(20);

            context.Animal.AddRange(animalFaker);
            context.SaveChanges();
        }
    }
}
