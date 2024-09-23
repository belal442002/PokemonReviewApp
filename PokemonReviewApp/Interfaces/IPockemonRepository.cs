using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IPockemonRepository
    {
        Task<ICollection<Pockemon>> GetPockemons();
        Task<Pockemon> GetPockemon(int id);
        Task<Pockemon> GetPockemon(String name);
        Task<decimal> GetPockemonRating(int id);
        Task<bool> PockemonExists(int id);
        Task<bool> PokemonExists(String name);
        Task<bool> CreatePokemon(int ownerId, int categoryId, Pockemon pokemon);
        Task<bool> UpdatePokemon(int ownerId, int categoryId, Pockemon pokemon);
        Task<bool> Save();
    }
}
