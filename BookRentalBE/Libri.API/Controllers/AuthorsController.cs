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
    public class AuthorsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAuthorService _authorService;
        public AuthorsController(IMapper mapper, IAuthorService authorService)
        {
            _mapper = mapper;
            _authorService = authorService;
        }

        [Cached(10000)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDTO>>> GetAllAuthors([FromQuery] string searchTerm)
        {
            var authors = await _authorService.GetAllAuthorsAsync(searchTerm);
            
            var response = _mapper.Map<IEnumerable<AuthorDTO>>(authors);
            
            return Ok(response);
        }

        [Cached(600)]
        [HttpGet("admin/authors-list")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Pagination<AuthorDTO>>> GetAllAuthorsForAdminAsync([FromQuery] AuthorParams authorParams)
        {
            var result = await _authorService.GetAllAuthorsForAdminAsync(authorParams);
            
            var authors = _mapper.Map<IEnumerable<AuthorDTO>>(result);
           
            var totalItems = result.FirstOrDefault()?.TotalCount ?? 0;
            
            var response = new Pagination<AuthorDTO>(authorParams.PageIndex, authorParams.PageSize, totalItems, authors);
            
            return Ok(response);
        }
        
        [InvalidateCache("api/Authors")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddAuthorAsync([FromBody] ModifyAuthorRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var author = _mapper.Map<Author>(request);
            
            var addAuthor = await _authorService.AddNewAuthorAsync(author);

            if (!addAuthor.Success)
            {
                return BadRequest(new APIResponse(400, addAuthor.Message));
            }
            
            return StatusCode(201, new { message = addAuthor.Message });
        }
        
        [InvalidateCache("api/Authors")]
        [HttpPost("import-from-file")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ImportAuthorsFromFileAsync([FromForm] IFormFile file)
        {
           var importAuthors = await _authorService.ImportAuthorsFromFileAsync(file);

           if (!importAuthors.Success)
           {
               return BadRequest(new APIResponse(400, importAuthors.Message, importAuthors.Errors));
           }

           return Ok(new APIResponse(201, importAuthors.Message));
        }
        
        [InvalidateCache("api/Authors")]
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateAuthorById([FromRoute] int id, [FromBody] ModifyAuthorRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var author = _mapper.Map<Author>(request);
            
            var updateAuthor = await _authorService.UpdateAuthorAsync(id, author);
            
            if (!updateAuthor.Success)
            {
                return BadRequest(new APIResponse(400, updateAuthor.Message));
            }

            return Ok(new { message = updateAuthor.Message });
        }
        
        [InvalidateCache("api/Authors")]  
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteAuthorById([FromRoute] int id)
        {
            var deleteAuthor = await _authorService.DeleteAuthorByIdAsync(id);
            
            if (!deleteAuthor.Success)
            {
                return BadRequest(new APIResponse(400, deleteAuthor.Message));
            }
            
            return Ok(new { message = deleteAuthor.Message });
        }

        [HttpGet("author-exists")]
        public async Task<ActionResult<bool>> CheckAuthorExistByNameAsync([FromQuery] string name)
        {
            var response = await _authorService.CheckAuthorExistByNameAsync(name);

            return Ok(response);
        }
    }
}
