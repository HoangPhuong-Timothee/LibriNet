using AutoMapper;
using Libri.API.DTOs.Request;
using Libri.API.DTOs.Response.Book;
using Libri.API.DTOs.Response.Errors;
using Libri.API.Helpers;
using Libri.BAL.Helpers.EntityParams;
using Libri.BAL.Services.Interfaces;
using Libri.DAL.Models.Custom.Pagination;
using Libri.DAL.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Libri.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenreSevice _genreService;
        private readonly IMapper _mapper;
        public GenresController(IMapper mapper, IGenreSevice genreService)
        {
            _mapper = mapper;
            _genreService = genreService;
        }

        [Cached(10000)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreDTO>>> GetAllGenresAsync()
        {
            var genres = await _genreService.GetAllGenresAsync();
            
            var response = _mapper.Map<IEnumerable<GenreDTO>>(genres);
            
            return Ok(response);
        }

        [Cached(600)]
        [HttpGet("admin/genres-list")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Pagination<GenreDTO>>> GetAllGenresForAdminAsync([FromQuery] GenreParams genreParams)
        {
            var result = await _genreService.GetAllGenresForAdminAsync(genreParams);
            
            var genres = _mapper.Map<IEnumerable<GenreDTO>>(result);
            
            var totalItems = result.FirstOrDefault()?.TotalCount ?? 0;
            
            var response = new Pagination<GenreDTO>(genreParams.PageIndex, genreParams.PageSize, totalItems, genres);
            
            return Ok(response);
        }
        
        [InvalidateCache("api/Genres")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddGenreAsync([FromBody] ModifyGenreRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var genre = _mapper.Map<Genre>(request);
            
            var addGenre = await _genreService.AddNewGenreAsync(genre);
            
            if (!addGenre.Success) 
            {
                return BadRequest(new APIResponse(400, addGenre.Message));
            }
            
            return StatusCode(201, new { message = addGenre.Message });
        }
        
        [InvalidateCache("api/Genres")]
        [HttpPost("import-from-file")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ImportGenresFromFileAsync([FromForm] IFormFile file)
        {
            var importGenres = await _genreService.ImportGenresFromFileAsync(file);
            
            if (!importGenres.Success) 
            {
                return BadRequest(new APIResponse(400, importGenres.Message, importGenres.Errors));
            }

            return StatusCode(201, new { message = importGenres.Message });
        }
        
        [InvalidateCache("api/Genres")]
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateGenreAsync([FromRoute] int id, [FromBody] ModifyGenreRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var genre = _mapper.Map<Genre>(request);
            
            var updateGenre = await _genreService.UpdateGenreAsync(id, genre);
            
            if (!updateGenre.Success) 
            {
                return BadRequest(new APIResponse(400, updateGenre.Message));
            }
            
            return Ok(new { message = updateGenre.Message });
        }
        
        [InvalidateCache("api/Genres")]
        [HttpDelete("soft-delete/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteGenreAsync([FromRoute] int id)
        {
            var deleteGenre = await _genreService.DeleteGenreByIdAsync(id);

            if (!deleteGenre.Success) 
            {
                return BadRequest(new APIResponse(400, deleteGenre.Message));
            }
            
            return Ok(new { message = deleteGenre.Message });
        }

        [HttpGet("genre-exists")]
        public async Task<ActionResult<bool>> CheckGenreExistByNameAsync([FromQuery] string name)
        {
            return await _genreService.CheckGenreExistByNameAsync(name);
        }
    }
}
