using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _context;

        public ReviewRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateReview(Review review)
        {
            await _context.Reviews.AddAsync(review);
            return await Save();
        }

        public async Task<Review> GetReview(int id)
        {
            return await _context.Reviews.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<ICollection<Review>> GetReviews()
        {
            return await _context.Reviews.ToListAsync();
        }

        public async Task<ICollection<Review>> GetReviewsOfPokemon(int pokemonId)
        {
            if (!await _context.Pockemon.AnyAsync(p => p.Id == pokemonId))
                throw new Exception("Not Found");

            return await _context.Reviews.Where(r => r.Pockemon.Id == pokemonId).ToListAsync();
        }

        public async Task<bool> ReviewExists(int id)
        {
            return await _context.Reviews.AnyAsync(r => r.Id == id);
        }

        public async Task<bool> ReviewExists(string title)
        {
            return await _context.Reviews.AnyAsync(r => r.Title.Trim().ToUpper() == title.Trim().ToUpper());
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }
    }
}
