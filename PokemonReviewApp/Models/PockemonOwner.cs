using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonReviewApp.Models
{
    [Table("PockemonOwner")]
    public class PockemonOwner
    {
        public int PockemonId { get; set; }
        public int OwnerId { get; set; }
        public Pockemon Pockemon { get; set; }
        public Owner Owner { get; set; }
    }
}
