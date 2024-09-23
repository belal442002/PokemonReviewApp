using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repositories
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly DataContext _context;

        public ReviewerRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateReviewer(Reviewer reviewer)
        {
            await _context.Reviewers.AddAsync(reviewer);
            return await Save();
        }

        public async Task<Reviewer> GetReviewer(int id)
        {
            return await _context.Reviewers.Where(r => r.Id == id).Include(r => r.Reviews).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Reviewer>> GetReviewrs()
        {
            return await _context.Reviewers.ToListAsync();
        }

        public async Task<ICollection<Review>> GetReviewsByReviewer(int reviewerId)
        {
            return await _context.Reviews.Where(r => r.Reviewer.Id == reviewerId).ToListAsync();
        }

        public async Task<bool> ReviewerExists(int id)
        {
            return await _context.Reviewers.AnyAsync(r => r.Id == id);
        }

        public async Task<bool> ReviewerExists(string firstName, string lastName)
        {
            return await _context.Reviewers
                .AnyAsync(r => r.FirstName.Trim().ToLower() == firstName.Trim().ToLower() && r.LastName.Trim().ToLower() == lastName.Trim().ToLower());
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }
    }
}
