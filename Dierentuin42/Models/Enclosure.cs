using System.ComponentModel.DataAnnotations;

namespace Dierentuin42.Models
{
    public class Enclosure
    {
        public int Id { get; set; }

        [Display(Name = "Naam")]
        public string Name { get; set; }

      
        public int? ZooId { get; set; }
        [Display(Name= "Dierentuin")]
        public Zoo? Zoo { get; set; }

        
        [Display(Name = "Dieren")]
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

        [Display(Name = "Klimaat")]
        public Climate EnclosureClimate { get; set; }

        [Display(Name = "Habitat Type")] 
        public HabitatType EnclosureHabitatType { get; set; } 

        [Display(Name = "Veiligheidsniveau")]
        public SecurityLevel EnclosureSecurityLevel { get; set; }

        [Display(Name = "Grootte (m²)")]
        public double Size { get; set; }
    }
}
