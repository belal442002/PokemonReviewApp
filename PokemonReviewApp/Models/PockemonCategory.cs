using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonReviewApp.Models
{
    [Table("PockemonCategory")]
    public class PockemonCategory
    {
        public int PockemonId { get; set; }
        public int CategoryId { get; set; }
        public Pockemon Pockemon { get; set; }
        public Category Category { get; set; }

    }
}
