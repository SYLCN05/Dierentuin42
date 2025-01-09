using System.ComponentModel.DataAnnotations;
using static Dierentuin42.Models.Enclosure;


namespace Dierentuin42.Models
{
    public class Animal
    {
        public int Id { get; set; }


        [Required(ErrorMessage ="Naam is verplicht")]
        [StringLength(20, ErrorMessage = "Naam mag niet langer zijn dan 20 tekens")]
        [Display(Name = "Naam")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Soort is verplicht.")]
        [StringLength(20, ErrorMessage = "Soort mag niet langer zijn dan 20 tekens.")]
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
        [StringLength(20, ErrorMessage = "Prooi naam mag niet langer zijn dan 20 tekens.")]
        [Display(Name = "Prooi")]
        public string Prey { get; set; }

        [Display(Name = "Verblijf")]
        public int? EnclosureId { get; set; }

        [Display(Name = "Verblijf")]
        public Enclosure? Enclosure { get; set; }

        [Required(ErrorMessage = "Ruimtevereiste is verplicht.")]
        [Range(1.0, 5000.0, ErrorMessage = "Voer een getal in tussen de 1 en 5000")]
        [Display(Name = "Ruimtevereiste (m²)")]
        public double spaceRequirement { get; set; }


        [Required(ErrorMessage = "Veiligheidsvereisten zijn verplicht.")]
        [Display(Name = "Veiligheidsvereisten")]
        public SecurityLevel? SecurityRequirement { get; set; }

        public enum SecurityLevel
        {
            Low,
            Medium,
            High
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

        public string GetFeedingTime()
        {
            Console.WriteLine($"Dier: {Name}, Voedsel: {Prey}, Dieet: {AnimalDiet}");

            if (!string.IsNullOrEmpty(Prey))
            {
                return $"Eet {Prey}.";
            }

            return AnimalDiet switch
            {
                DietaryClass.Carnivore => "Eet vlees.",
                DietaryClass.Herbivore => "Eet planten.",
                DietaryClass.Omnivore => "Eet zowel planten als vlees.",
                DietaryClass.Insectivore => "Eet insecten.",
                DietaryClass.Piscivore => "Eet vis.",
                _ => "Geen specifiek dieet."
            };
        }

        public bool HasValidPrey() => !string.IsNullOrEmpty(Prey);

        public bool HasValidCategory()
        {
            return CategoryId.HasValue && CategoryId.Value != 0;
        }

        public bool HasValidEnclosure()
        {
            return EnclosureId.HasValue && EnclosureId.Value != 0;
        }
        public bool HasValidDiet() => Enum.IsDefined(typeof(DietaryClass), AnimalDiet);

        public bool HasValidSize() => Enum.IsDefined(typeof(Size), AnimalSize);

        public bool HasValidActivityPattern() => Enum.IsDefined(typeof(ActivityPattern), AnimalActivityPattern);

        public bool HasValidSecurityLevel() => Enum.IsDefined(typeof(SecurityLevel), SecurityRequirement);

        public Dictionary<string, bool> CheckAllConstraints()
        {
            var constraints = new Dictionary<string, bool>
            {
                { "Heeft prooi", HasValidPrey() },
                { "Heeft een categorie", HasValidCategory() },
                { "Is gekoppeld aan een verblijf", HasValidEnclosure() },
                { "Geldig dieet", HasValidDiet() },
                { "Geldige grootte", HasValidSize() },
                { "Geldig activiteitenpatroon", HasValidActivityPattern() },
                { "Beveiligingsniveau aanwezig", HasValidSecurityLevel() }
            };

            return constraints;
        }

    }
}
