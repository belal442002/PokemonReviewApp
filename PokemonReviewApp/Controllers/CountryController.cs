using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public async Task<IActionResult> GetContries()
        {
            var countries = _mapper.Map<List<CountryDto>>(await _countryRepository.GetCountries());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(countries);
        }

        [HttpGet("{countryId:int}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCountry(int countryId)
        {
            if (!await _countryRepository.CountryExists(countryId))
                return NotFound();

            var country = _mapper.Map<CountryDto>(await _countryRepository.GetCountry(countryId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(country);
        }

        [HttpGet("byOwner/{ownerId:int}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCountryByOwner(int ownerId)
        {
            try
            {
                var country = _mapper.Map<CountryDto>(await _countryRepository.GetCountryByOwner(ownerId));

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(country);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("getOwners/{countryId:int}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetOwnersByCountry(int countryId)
        {
            if (!await _countryRepository.CountryExists(countryId))
                return NotFound();

            var owners = _mapper.Map<List<OwnerDto>>(await _countryRepository.GetOwnersByCountry(countryId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owners);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCountry([FromBody] CountryDto countryDto)
        {
            if (countryDto == null)
                return BadRequest(ModelState);

            if(await _countryRepository.CountryExists(countryDto.Name))
            {
                ModelState.AddModelError("", "Country already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var country = _mapper.Map<Country>(countryDto);

            if(! await _countryRepository.CreateCountry(country))
            {
                ModelState.AddModelError("", "Somthing went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{countryId:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCountry(int countryId, [FromBody]CountryDto updatedCountry)
        {
            if (updatedCountry == null)
                return BadRequest(ModelState);

            if (countryId != updatedCountry.Id)
                return BadRequest(ModelState);

            if (!await _countryRepository.CountryExists(countryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var country = _mapper.Map<Country>(updatedCountry);

            if(!await _countryRepository.UpdateCountry(country))
            {
                ModelState.AddModelError("", "Somthing went wrong while saving country");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
