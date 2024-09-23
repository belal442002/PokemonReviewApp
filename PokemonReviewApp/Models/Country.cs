using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonReviewApp.Models
{
    [Table("Country")]
    public class Country
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public ICollection<Owner> Owners { get; set; }
    }
}
