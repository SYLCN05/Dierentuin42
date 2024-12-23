using System.ComponentModel.DataAnnotations;

namespace Dierentuin42.Models
{
    public class Zoo
    {
        public int Id { get; set; }

        [Display(Name = "Dierentuin naam")]
        public string Name { get; set; }

        [Display(Name = "Verblijf/verblijven:")]
        public List<Enclosure> Enclosures { get; set; } = new List<Enclosure>();


    }
}
