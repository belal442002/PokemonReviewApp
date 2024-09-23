using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext _context;

        public CountryRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<Country> GetCountryByOwner(int ownerId)
        {
            if (!await _context.Owners.AnyAsync(o => o.Id == ownerId))
                throw new Exception("Not Found");
            return await _context.Owners.Where(o => o.Id == ownerId).Select(o => o.Country).FirstOrDefaultAsync();
            
        }

        public async Task<ICollection<Country>> GetCountries()
        {
            return await _context.Countries.OrderBy(c => c.Id).ToListAsync();
        }

        public async Task<Country> GetCountry(int id)
        {
            return await _context.Countries.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<ICollection<Owner>> GetOwnersByCountry(int countryId)
        {
            return await _context.Countries.Where(c => c.Id == countryId).Select(c => c.Owners).FirstOrDefaultAsync();
            //return await _contxt.Owners.Where(o => o.Country.Id == countryId).ToListAsync();
        }

        public async Task<bool> CountryExists(int id)
        {
            return await _context.Countries.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CountryExists(string countryName)
        {
            return await _context.Countries.AnyAsync(c => c.Name.Trim().ToUpper() == countryName.Trim().ToUpper());
        }

        public async Task<bool> CreateCountry(Country country)
        {
            await _context.Countries.AddAsync(country);
            return await Save();
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> UpdateCountry(Country country)
        {
            _context.Countries.Update(country);
            return await Save();
        }
    }
}
