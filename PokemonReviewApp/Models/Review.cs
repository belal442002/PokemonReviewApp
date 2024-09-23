using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonReviewApp.Models
{
    [Table("Review")]
    public class Review
    {
        public int Id { get; set; }
        public String Title { get; set; }
        public String Text { get; set; }
        public int Rating { get; set; }
        public Reviewer Reviewer { get; set; }
        public Pockemon Pockemon { get; set; }
    }
}
