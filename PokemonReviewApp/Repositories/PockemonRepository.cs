using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using System.Reflection.Metadata.Ecma335;

namespace PokemonReviewApp.Repositories
{
    public class PockemonRepository : IPockemonRepository
    {
        private readonly DataContext _context;

        public PockemonRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> CreatePokemon(int ownerId, int categoryId, Pockemon pokemon)
        {

            var owner = await _context.Owners.FirstOrDefaultAsync(o => o.Id == ownerId);
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);

            var pokemonCategory = new PockemonCategory()
            {
                CategoryId = categoryId,
                PockemonId = pokemon.Id,
                Category = category!,
                Pockemon = pokemon
            };

            await _context.PockemonCategories.AddAsync(pokemonCategory);

            var pokemonOwner = new PockemonOwner()
            {
                PockemonId = pokemon.Id,
                OwnerId = ownerId,
                Owner = owner!,
                Pockemon = pokemon
            };

            await _context.PockemonOwners.AddAsync(pokemonOwner);
            await _context.Pockemon.AddAsync(pokemon);

            return await Save();

        }

        public async Task<Pockemon> GetPockemon(int id)
        {
            return await _context.Pockemon.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Pockemon> GetPockemon(string name)
        {
            return await _context.Pockemon.FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<decimal> GetPockemonRating(int id)
        {
            var reviews = await _context.Reviews.Where(r => r.Pockemon.Id == id).ToListAsync();
            if (reviews.Count() <= 0)
                return 0;
            return ((decimal) reviews.Sum(r => r.Rating)/ reviews.Count());
        }

        public async Task<ICollection<Pockemon>> GetPockemons()
        {
            return await _context.Pockemon.OrderBy(p => p.Id).ToListAsync();
        }

        public async Task<bool> PockemonExists(int id)
        {
            //var pokemon = _context.Pockemon.Where(p => p.Id == id).FirstOrDefault();
            //if (pokemon is null)
            //    return false;
            //return true;
            return await _context.Pockemon.AnyAsync(p => p.Id == id);
        }

        public async Task<bool> PokemonExists(string name)
        {
            return await _context.Pockemon.AnyAsync(p => p.Name.Trim().ToUpper() == name.Trim().ToUpper());
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;

        }

        public async Task<bool> UpdatePokemon(int ownerId, int categoryId, Pockemon pokemon)
        {
            var existingPokemon = await _context.Pockemon.FirstOrDefaultAsync(p => p.Id == pokemon.Id);
            existingPokemon!.Name = pokemon.Name;
            existingPokemon.BirthDate = pokemon.BirthDate;

            await _context.SaveChangesAsync();

            await _context.PockemonCategories.Where(pc => pc.PockemonId == pokemon.Id)
               .ExecuteUpdateAsync(pc => pc
               .SetProperty(pc => pc.Pockemon, existingPokemon));

            await _context.PockemonOwners.Where(po => po.PockemonId == pokemon.Id)
                .ExecuteUpdateAsync(po => po
                .SetProperty(po => po.Pockemon, existingPokemon));

            return await Save();
        }
    }
}
