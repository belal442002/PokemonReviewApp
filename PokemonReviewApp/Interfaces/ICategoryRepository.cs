using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface ICategoryRepository
    {
        Task<ICollection<Category>> GetCategories();
        Task<Category> GetCategory(int id);
        Task<ICollection<Pockemon>> GetPokemonsByCategory(int CategoryId);
        Task<bool> CategoryExists(int id);
        Task<bool> CategoryExists(String categoryName);
        Task<bool> CreateCategory(Category category);
        Task<bool> UpdateCategory(Category category);
        Task<bool> Save();
    }
}
