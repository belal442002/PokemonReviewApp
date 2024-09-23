using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IOwnerRepository
    {
        Task<ICollection<Owner>> GetOwners();
        Task<Owner> GetOwner(int id);
        Task<ICollection<Owner>> GetOwnersByPokemon(int pokemonId);
        Task<ICollection<Pockemon>> GetPokwmonsByOwner(int ownerId);
        Task<bool> OwnerExists(int ownerId);
        Task<bool> OwnerExists(String firstName, String lastName);
        Task<bool> CreateOwner(Owner owner);
        Task<bool> UpdateOwner(Owner owner);
        Task<bool> Save();
    }
}
