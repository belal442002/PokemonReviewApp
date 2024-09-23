using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IReviewerRepository
    {
        Task<Reviewer> GetReviewer(int id);
        Task<ICollection<Reviewer>> GetReviewrs();
        Task<ICollection<Review>> GetReviewsByReviewer(int reviewerId);
        Task<bool> ReviewerExists(int id);
        Task<bool> ReviewerExists(String firstName, String lastName);
        Task<bool> CreateReviewer(Reviewer reviewer);
        Task<bool> Save();
       
    }
}
