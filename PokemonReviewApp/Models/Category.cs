using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonReviewApp.Models
{
    [Table("Category")]
    public class Category
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public ICollection<PockemonCategory> pockemonCategories { get; set; }
    }
}
