using System.ComponentModel.DataAnnotations;
using static Dierentuin42.Models.Enclosure;


namespace Dierentuin42.Models
{
    public class Animal
    {
        public int Id { get; set; }


        [Required(ErrorMessage ="Naam is verplicht")]
        [StringLength(50, ErrorMessage ="Naam mag niet langer zijn dan 50 tekens")]
        [Display(Name = "Naam")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Soort is verplicht.")]
        [StringLength(50, ErrorMessage = "Soort mag niet langer zijn dan 50 tekens.")]
        [Display(Name = "Soort")]
        public string Species { get; set; }

        [Display(Name = "Categorie")]
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

        [Required(ErrorMessage = "Grootte van het dier is verplicht.")]
        [Display(Name = "Grootte")]
        public Size? AnimalSize { get; set; }

        public enum DietaryClass
        {
            Carnivore,
            Herbivore,
            Omnivore,
            Insectivore,
            Piscivore
        }

        [Required(ErrorMessage = "Dieet is verplicht.")]
        [Display(Name = "Dieet")]
        public DietaryClass? AnimalDiet { get; set; }

        public enum ActivityPattern
        {
            Diurnal,
            Nocturnal,
            Cathemeral
        }

        [Required(ErrorMessage = "Activiteits patroon is verplicht.")]
        [Display(Name = "Activiteitspatroon")]
        public ActivityPattern? AnimalActivityPattern { get; set; }

        [Required(ErrorMessage = "Prooi is verplicht.")]
        [StringLength(50, ErrorMessage = "Prooi naam mag niet langer zijn dan 50 tekens.")]
        [Display(Name = "Prooi")]
        public string Prey { get; set; }

        [Display(Name = "Verblijf")]
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
                    _ => 10.0                 
                };


            }
        }

        [Required(ErrorMessage = "Veiligheidsvereisten zijn verplicht.")]
        [Display(Name = "Veiligheidsvereisten")]
        public SecurityLevel? SecurityRequirement { get; set; }
    }
}
