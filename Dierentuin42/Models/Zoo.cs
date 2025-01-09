using System.ComponentModel.DataAnnotations;

namespace Dierentuin42.Models
{
    public class Zoo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Dierentuin naam is verplicht.")]
        [StringLength(30, ErrorMessage = "Dierentuin naam mag niet langer zijn dan 30 tekens.")]
        [Display(Name = "Naam")]
        public string Name { get; set; }

        [Display(Name = "Verblijf/verblijven:")]
        public List<Enclosure> Enclosures { get; set; } = new List<Enclosure>();


    }
}
