using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repositories;

namespace PokemonReviewApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PockemonController : ControllerBase
    {
        private readonly IPockemonRepository _pockemonRepository;
        private readonly IMapper _mapper;

        public PockemonController(IPockemonRepository pockemonRepository, IMapper mapper)
        {
            _pockemonRepository = pockemonRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pockemon>))]
        public async Task<IActionResult> GetPockemons()
        {
            var pokemons = _mapper.Map<List<PokemonDto>>(await _pockemonRepository.GetPockemons());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemons);
        }

        [HttpGet("{pokeId:int}")]
        [ProducesResponseType(200, Type = typeof(Pockemon))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPockemon(int pokeId)
        {
            if (!await _pockemonRepository.PockemonExists(pokeId))
                return NotFound();
            var pokemon = _mapper.Map<PokemonDto>(await _pockemonRepository.GetPockemon(pokeId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemon);
        }

        [HttpGet("{pokeId:int}/rating")]
        [ProducesResponseType(200, Type = typeof(Pockemon))]
        [ProducesResponseType(400)]
        [Produces("application/json")]
        public async Task<IActionResult> GetPockemonRating(int pokeId)
        {
            if (!await _pockemonRepository.PockemonExists(pokeId))
                return NotFound();

            var rating = await _pockemonRepository.GetPockemonRating(pokeId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(rating);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreatePokemon([FromQuery]int ownerId, [FromQuery]int categoryId, [FromBody]PokemonDto pokemonDto)
        {
            if (pokemonDto == null)
                return BadRequest(ModelState);
            if(await _pockemonRepository.PokemonExists(pokemonDto.Name))
            {
                ModelState.AddModelError("", "Pokemon already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokemon = _mapper.Map<Pockemon>(pokemonDto);

            if(!await _pockemonRepository.CreatePokemon(ownerId, categoryId, pokemon))
            {
                ModelState.AddModelError("", "Somthing went wrong while saving pokemon");
                return StatusCode(500, ModelState);
            }

            return Ok("Saving successfully");
        }

        [HttpPut("{pokemonId:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdatePokemon(int pokemonId, [FromQuery]int ownerId, [FromQuery]int categoryId, PokemonDto updatedPokemon)
        {
            if (updatedPokemon == null)
                return BadRequest(ModelState);

            if (pokemonId != updatedPokemon.Id)
                return BadRequest(ModelState);

            if (!await _pockemonRepository.PockemonExists(pokemonId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokemon = _mapper.Map<Pockemon>(updatedPokemon);

            if(!await _pockemonRepository.UpdatePokemon(ownerId, categoryId, pokemon))
            {
                ModelState.AddModelError("", "Somthing went wrong while saving pokemon");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
