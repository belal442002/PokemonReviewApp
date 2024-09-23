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
    public class ReviewerController : ControllerBase
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
        {
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }
        [HttpGet("{reviewerId:int}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetReviewer(int reviewerId)
        {
            if (!await _reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();
            
            var reviewer = _mapper.Map<ReviewerDto>(await _reviewerRepository.GetReviewer(reviewerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviewer);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetReviewers()
        {
            var reviewers = _mapper.Map<List<ReviewerDto>>(await _reviewerRepository.GetReviewrs());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviewers);
        }

        [HttpGet("{reviewerID:int}/reviews")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetReviewsByReviewer(int reviewerID)
        {
            if (!await _reviewerRepository.ReviewerExists(reviewerID))
                return NotFound();

            var reviews = _mapper.Map<List<Review>>(await _reviewerRepository.GetReviewsByReviewer(reviewerID));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateReviewer([FromBody]ReviewerDto reviewerDto)
        {
            if (reviewerDto == null)
                return BadRequest(ModelState);

            if(await _reviewerRepository.ReviewerExists(reviewerDto.FirstName, reviewerDto.LastName))
            {
                ModelState.AddModelError("", "Reviewer already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewer = _mapper.Map<Reviewer>(reviewerDto);

            if(!await _reviewerRepository.CreateReviewer(reviewer))
            {
                ModelState.AddModelError("", "Error happend while saving reviewer");
                return StatusCode(500, ModelState);
            }

            return Ok("Succussfully created");
        }

    }
}
