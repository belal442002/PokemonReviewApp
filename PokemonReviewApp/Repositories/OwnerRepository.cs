using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _contex;

        public OwnerRepository(DataContext contex)
        {
            _contex = contex;
        }

        public async Task<bool> CreateOwner(Owner owner)
        {
            await _contex.Owners.AddAsync(owner);
            return await Save();
        }

        public async Task<Owner> GetOwner(int id)
        {
            return await _contex.Owners.FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<ICollection<Owner>> GetOwners()
        {
            return await _contex.Owners.OrderBy(o => o.Id).ToListAsync();
        }

        public async Task<ICollection<Owner>> GetOwnersByPokemon(int pokemonId)
        {
            if (!await _contex.Pockemon.AnyAsync(p => p.Id == pokemonId))
                throw new Exception("Not Found");
            return await _contex.PockemonOwners.Where(po => po.PockemonId == pokemonId).Select(po => po.Owner).ToListAsync();
        }

        public async Task<ICollection<Pockemon>> GetPokwmonsByOwner(int ownerId)
        {
            return await _contex.PockemonOwners.Where(po => po.OwnerId == ownerId).Select(po => po.Pockemon).ToListAsync();
        }

        public async Task<bool> OwnerExists(int ownerId)
        {
            return await _contex.Owners.AnyAsync(o => o.Id == ownerId);
        }

        public async Task<bool> OwnerExists(string firstName, String lastName)
        {
            return await _contex.Owners.AnyAsync(o => o.FirstName.Trim().ToUpper() == firstName.Trim().ToUpper() && o.LastName.Trim().ToUpper() == lastName.Trim().ToUpper());
        }

        public async Task<bool> Save()
        {
            var saved = await _contex.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> UpdateOwner(Owner owner)
        {
            _contex.Owners.Update(owner);
            return await Save();
        }
    }
}
