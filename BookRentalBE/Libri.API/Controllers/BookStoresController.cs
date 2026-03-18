using AutoMapper;
using Libri.API.DTOs.Request;
using Libri.API.DTOs.Response.Errors;
using Libri.API.DTOs.Response.Inventory;
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
    [Authorize(Roles = "Admin")]
    public class BookStoresController : ControllerBase
    {
        private readonly IBookStoreService _bookStoreService;
        private readonly IMapper _mapper;
        public BookStoresController(IBookStoreService bookStoreService, IMapper mapper)
        {
            _bookStoreService = bookStoreService;
            _mapper = mapper;
        }

        [Cached(600)]
        [HttpGet("admin/bookstores-list")]
        public async Task<ActionResult<Pagination<BookStoreDTO>>> GetAllBookStoreForAdminAsync([FromQuery] BookStoreParams bookStoreParams)
        {
            var result = await _bookStoreService.GetAllBookStoresForAdminAsync(bookStoreParams);

            var bookStores = _mapper.Map<IEnumerable<BookStoreDTO>>(result);

            var totalItems = result.FirstOrDefault()?.TotalCount ?? 0;

            var response = new Pagination<BookStoreDTO>(bookStoreParams.PageIndex, bookStoreParams.PageSize, totalItems, bookStores);

            return Ok(response);
        }

        [Cached(10000)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookStoreDTO>>> GetAllBookStoreAsync()
        {
            var bookStores = await _bookStoreService.GetAllBookStoresAsync();

            var response = _mapper.Map<IEnumerable<BookStoreDTO>>(bookStores);

            return Ok(response);
        }

        [InvalidateCache("api/BookStores")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddBookStoreAsync([FromBody] ModifyBookStoreRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bookStore = _mapper.Map<BookStore>(request);

            var addBookStore = await _bookStoreService.AddNewBookStoreAsync(bookStore);

            if (!addBookStore.Success)
            {
                return BadRequest(new APIResponse(400, addBookStore.Message));
            }

            return StatusCode(201, new { message = addBookStore.Message });
        }

        [InvalidateCache("api/BookStores")]
        [HttpPost("import-from-file")]
        public async Task<ActionResult> ImportBookStoresFromFileAsync([FromForm] IFormFile file)
        {
            var importBookStores = await _bookStoreService.ImportBookStoresFromFileAsync(file);

            if (!importBookStores.Success)
            {
                return BadRequest(new APIResponse(400, importBookStores.Message, importBookStores.Errors));
            }

            return StatusCode(201, new { message = importBookStores.Message });
        }
        
        [InvalidateCache("api/BookStores")]
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateBookStoreAsync([FromRoute] int id, [FromBody] ModifyBookStoreRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bookStore = _mapper.Map<BookStore>(request);

            var updateBookStore = await _bookStoreService.UpdateBookStoreAsync(id, bookStore);

            if (!updateBookStore.Success)
            {
                return BadRequest(new APIResponse(400, updateBookStore.Message));
            }

            return Ok(new { message = updateBookStore.Message });
        }   
        
        [InvalidateCache("api/BookStores")]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteBookAsync([FromRoute] int id)
        {
            var deleteBookStore = await _bookStoreService.DeleteBookStoreByIdAsync(id);

            if (!deleteBookStore.Success)
            {
                return BadRequest(new APIResponse(400, deleteBookStore.Message));
            }

            return Ok(new { message = deleteBookStore.Message });
        }
        
        [HttpGet("bookstore-exists")]
        public async Task<ActionResult<bool>> CheckBookStoreIsExistAsync([FromQuery] string storeName)
        {
            return await _bookStoreService.CheckBookStoreExistByStoreNameAsync(storeName);
        }
    }
}
