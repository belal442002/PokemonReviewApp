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
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public OwnerController(IOwnerRepository ownerRepository, ICountryRepository countryRepository , IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerator<Owner>))]
        public async Task<IActionResult> GetOwners()
        {
            var owners = _mapper.Map<List<OwnerDto>>(await _ownerRepository.GetOwners());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owners);
        }

        [HttpGet("{ownerId:int}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetOwner(int ownerId)
        {
            if (!await _ownerRepository.OwnerExists(ownerId))
                return NotFound();

            var owner = _mapper.Map<OwnerDto>(await _ownerRepository.GetOwner(ownerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owner);
        }

        [HttpGet("byPokemon/{pokemonId:int}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetOwnersByPokemon(int pokemonId)
        {
            try
            {
                var owners = _mapper.Map<List<OwnerDto>>(await _ownerRepository.GetOwnersByPokemon(pokemonId));

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(owners);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("getPokemons/{ownerId:int}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pockemon>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPokemonsByOwner(int ownerId)
        {
            if (!await _ownerRepository.OwnerExists(ownerId))
                return NotFound();

            var pokemons = _mapper.Map<List<PokemonDto>>(await _ownerRepository.GetPokwmonsByOwner(ownerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemons);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateOwner([FromQuery]int countryId, [FromBody]OwnerDto ownerDto)
        {
            if (ownerDto == null)
                return BadRequest(ModelState);

            if (await _ownerRepository.OwnerExists(ownerDto.FirstName, ownerDto.LastName))
            {
                ModelState.AddModelError("", "Owner already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var owner = _mapper.Map<Owner>(ownerDto);
            owner.Country = await _countryRepository.GetCountry(countryId);

            if(!await _ownerRepository.CreateOwner(owner))
            {
                ModelState.AddModelError("", "Somthing went wrong while saving owenr");
                return StatusCode(500, ModelState);
            }

            return Ok("Created successfully");
        }

        [HttpPut("{ownerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateOwner(int ownerId, [FromBody]OwnerDto updatedOwner)
        {
            if (updatedOwner == null)
                return BadRequest(ModelState);

            if (ownerId != updatedOwner.Id)
                return BadRequest(ModelState);

            if (!await _ownerRepository.OwnerExists(ownerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var owner = _mapper.Map<Owner>(updatedOwner);

            if(!await _ownerRepository.UpdateOwner(owner))
            {
                ModelState.AddModelError("", "Somthing went wrong while saving owner");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
