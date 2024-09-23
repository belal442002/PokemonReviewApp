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
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IPockemonRepository _pockemonRepository;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewController(IReviewRepository reviewRepository, IPockemonRepository pockemonRepository, IReviewerRepository reviewerRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _pockemonRepository = pockemonRepository;
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        public async Task<IActionResult> GetReviews()
        {
            var reviews = _mapper.Map<List<ReviewDto>>(await _reviewRepository.GetReviews());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviews);
        }

        [HttpGet("{reviewId:int}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetReview(int reviewId)
        {
            if (!await _reviewRepository.ReviewExists(reviewId))
                return NotFound();

            var review = _mapper.Map<ReviewDto>(await _reviewRepository.GetReview(reviewId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(review);
        }

        [HttpGet("ofPokemon/{pokemonId:int}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetReviewsOfPokemon(int pokemonId)
        {
            try
            {
                var reviews = _mapper.Map<List<ReviewDto>>(await _reviewRepository.GetReviewsOfPokemon(pokemonId));

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(reviews);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateReview([FromQuery] int reviewerId, [FromQuery] int pokemonId, [FromBody] ReviewDto reviewDto)
        {
            if (reviewDto == null)
                return BadRequest(ModelState);

            if (await _reviewRepository.ReviewExists(reviewDto.Title))
            {
                ModelState.AddModelError("", "Review already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var review = _mapper.Map<Review>(reviewDto);
            review.Reviewer = await _reviewerRepository.GetReviewer(reviewerId);
            review.Pockemon = await _pockemonRepository.GetPockemon(pokemonId);

            if (!await _reviewRepository.CreateReview(review))
            {
                ModelState.AddModelError("", "Somthing went wrong");
                return StatusCode(500, ModelState);
            }

            return CreatedAtAction(nameof(GetReview), new { reviewId = reviewDto.Id}, reviewDto);

        }
    }
}
