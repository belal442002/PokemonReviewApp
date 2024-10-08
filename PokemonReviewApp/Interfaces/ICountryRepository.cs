﻿using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface ICountryRepository
    {
        Task<ICollection<Country>> GetCountries();
        Task<Country> GetCountry(int id);
        Task<Country> GetCountryByOwner(int ownerId);
        Task<ICollection<Owner>> GetOwnersByCountry(int countryId);
        Task<bool> CountryExists(int id);
        Task<bool> CountryExists(String countryName);
        Task<bool> CreateCountry(Country country);
        Task<bool> UpdateCountry(Country country);
        Task<bool> Save();
    }
}
