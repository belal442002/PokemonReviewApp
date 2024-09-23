using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonReviewApp.Models
{
    [Table("Owner")]
    public class Owner
    {
        public int Id { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Gym { get; set; }
        public Country Country { get; set; }
        public ICollection<PockemonOwner> PockemonOwners { get; set; }
    }
}
