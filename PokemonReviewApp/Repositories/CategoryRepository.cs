using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> CategoryExists(string categoryName)
        {
            return await _context.Categories.AnyAsync(c => c.Name.Trim().ToUpper() == categoryName.Trim().ToUpper());
        }

        public async Task<bool> CategoryExists(int id)
        {
            return await _context.Categories.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CreateCategory(Category category)
        {
            await _context.Categories.AddAsync(category);
            return await Save();
        }

        public async Task<ICollection<Category>> GetCategories()
        {
            return await _context.Categories.OrderBy(c => c.Id).ToListAsync();
        }

        public async Task<Category> GetCategory(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<ICollection<Pockemon>> GetPokemonsByCategory(int CategoryId)
        {
            return await _context.PockemonCategories.Where(pc => pc.CategoryId == CategoryId).Select(pc => pc.Pockemon).ToListAsync();
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            return await Save();
        }
    }
}
