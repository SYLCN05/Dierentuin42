using System.ComponentModel.DataAnnotations;
using static Dierentuin42.Models.Enclosure;


namespace Dierentuin42.Models
{
    public class Animal
    {
        public int Id { get; set; }



        [Display(Name = "Naam")]
        public string Name { get; set; }



        [Display(Name = "Soort")]
        public string Species { get; set; }

        public int? CategoryId { get; set; }

        [Display(Name = "Categorie")]
        public Category? Category { get; set; }



        public enum Size
        {
            Microscopic,
            VerySmall,
            Small,
            Medium,
            Large,
            VeryLarge

        }
        [Display(Name = "Grootte van het dier")]
        public Size AnimalSize { get; set; }

        public enum DietaryClass
        {
            Carnivore,
            Herbivore,
            Omnivore,
            Insectivore,
            Piscivore
        }
        [Display(Name = "Dieet")]
        public DietaryClass AnimalDiet { get; set; }

        public enum ActivityPattern
        {
            Diurnal,
            Nocturnal,
            Cathemeral
        }
        [Display(Name = "Activiteits patroon")]
        public ActivityPattern AnimalActivityPattern { get; set; }

        [Display(Name = "Prooi")]
        public string Prey { get; set; }

        public int? EnclosureId { get; set; }

        [Display(Name = "Verblijf")]
        public Enclosure? Enclosure { get; set; }

        [Range(0.01, 100.0, ErrorMessage = "Voer een getal in tussen de 0,01 en 100,0")]
        [Display(Name = "Ruimtevereiste (m²)")]
        public double SpaceRequirement
        {
            get
            {
                return AnimalSize switch
                {
                    Size.Microscopic => SpaceRequirements.Microscopic,
                    Size.VerySmall => SpaceRequirements.VerySmall,
                    Size.Small => SpaceRequirements.Small,
                    Size.Medium => SpaceRequirements.Medium,
                    Size.Large => SpaceRequirements.Large,
                    Size.VeryLarge => SpaceRequirements.VeryLarge,
                    _ => 10.0                 // default ruimte
                };


            }
        }

        public bool IsAwake(bool isSunrise)
        {
            return AnimalActivityPattern switch
            {
                ActivityPattern.Diurnal => isSunrise,
                ActivityPattern.Nocturnal => !isSunrise,
                ActivityPattern.Cathemeral => true,
                _ => false
            };
        }

        [Display(Name = "VeiligheidsVereisten")]
        public SecurityLevel SecurityRequirement { get; set; }

    }
}
