using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IReviewRepository
    {
        Task<ICollection<Review>> GetReviews();
        Task<Review> GetReview(int id);
        Task<ICollection<Review>> GetReviewsOfPokemon(int pokemonId);
        Task<bool> ReviewExists(int id);
        Task<bool> ReviewExists(String title);
        Task<bool> CreateReview(Review review);
        Task<bool> Save();
    }
}
