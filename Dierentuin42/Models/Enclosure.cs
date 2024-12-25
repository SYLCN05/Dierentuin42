using System.ComponentModel.DataAnnotations;

namespace Dierentuin42.Models
{
    public class Enclosure
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naam is verplicht.")]
        [StringLength(50, ErrorMessage = "Naam mag niet langer zijn dan 50 tekens.")]
        [Display(Name = "Naam")]
        public string Name { get; set; }

        public int? ZooId { get; set; }

        [Display(Name= "Dierentuin")]
        public Zoo? Zoo { get; set; }


        [Display(Name = "Dieren in dit verblijf")]
        public List<Animal> Animals { get; set; } = new List<Animal>();

        public enum Climate
        {
            Tropical,
            Temperate,
            Arctic
        }

        [Flags]
        public enum HabitatType
        {
           
            Forest = 1,
            Aquatic = 2,
            Desert = 4,
            Grassland = 8
        }

        public enum SecurityLevel
        {
            Low,
            Medium,
            High
        }

        [Required(ErrorMessage = "Klimaat is verplicht.")]
        [Display(Name = "Klimaat")]
        public Climate EnclosureClimate { get; set; }

        [Required(ErrorMessage = "Habitat type is verplicht.")]
        [Display(Name = "Habitat Type")] 
        public HabitatType EnclosureHabitatType { get; set; }

        [Required(ErrorMessage = "Veiligheidsniveau is verplicht.")]
        [Display(Name = "Veiligheidsniveau")]
        public SecurityLevel EnclosureSecurityLevel { get; set; }

        [Range(0.01, 1000.0, ErrorMessage = "Grootte moet tussen 0,01 en 1000 m² liggen.")]
        [Display(Name = "Grootte (m²)")]
        public double Size { get; set; }
    }
}
