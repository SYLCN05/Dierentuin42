using System.ComponentModel.DataAnnotations;

namespace Dierentuin42.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naam is verplicht.")]
        [StringLength(50, ErrorMessage = "Naam mag niet langer zijn dan 50 tekens.")]
        [Display(Name = "Naam")]
        public string Name { get; set; }

        [Display(Name = "Dieren in deze categorie")]
        public List<Animal> Animals { get; set; } = new List<Animal>();


    }
}
