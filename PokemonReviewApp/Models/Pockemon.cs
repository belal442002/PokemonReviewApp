using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonReviewApp.Models
{
    [Table("Pockemon")]
    public class Pockemon
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public DateTime BirthDate { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<PockemonCategory> PockemonCategories { get; set; }
        public ICollection<PockemonOwner> PockemonOwners { get; set; }
    }
}
