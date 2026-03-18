using AutoMapper;
using Libri.API.DTOs.Response.Book;
using Libri.API.DTOs.Response.Errors;
using Libri.API.Helpers;
using Libri.BAL.Helpers.EntityParams;
using Libri.BAL.Services.Interfaces;
using Libri.DAL.Models.Custom.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Libri.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBookService _bookService;
        public BooksController(IMapper mapper, IBookService bookService)
        {
            _mapper = mapper;
            _bookService = bookService;
        }

        [Cached(600)]
        [HttpGet]
        public async Task<ActionResult<Pagination<BookDTO>>> GetAllBooksAsync([FromQuery] BookParams bookParams)
        {
            var result = await _bookService.GetAllBooksAsync(bookParams);

            var books = _mapper.Map<IEnumerable<BookDTO>>(result);

            var totalItems = result.FirstOrDefault()?.TotalCount ?? 0;

            var response = new Pagination<BookDTO>(bookParams.PageIndex, bookParams.PageSize, totalItems, books);

            return Ok(response);
        }

        [Cached(600)]
        [HttpGet("latest")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetLatestBooksAsync()
        {
            var latestBooks = await _bookService.GetLatestBooksAsync();

            var response = _mapper.Map<IEnumerable<BookDTO>>(latestBooks);

            return Ok(response);
        }

        [Cached(600)]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookDTO>> GetBookByIdAsync([FromRoute] int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);

            if (book == null)
            {
                return NotFound(new APIResponse(404));
            }

            var response = _mapper.Map<BookDTO>(book);

            return Ok(response);
        }

        [Cached(600)]
        [HttpGet("{id:int}/similar")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetSimilarBooksAsync([FromRoute] int id)
        {
            var similarBooks = await _bookService.GetSimilarBooksAsync(id);

            var response = _mapper.Map<IEnumerable<BookDTO>>(similarBooks);

            return Ok(response);
        }

        [InvalidateCache("api/Books")]
        [HttpPost("import-from-file")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ImportBooksFromFileAsync([FromForm] IFormFile file)
        {
            var importBooks = await _bookService.ImportBooksFromFileAsync(file);

            if (!importBooks.Success)
            {
                return BadRequest(new APIResponse(400, importBooks.Message, importBooks.Errors!));
            }

            return StatusCode(201, new { message = importBooks.Message });
        }

        [InvalidateCache("api/Books")]
        [HttpDelete("soft-delete/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteBookAsync([FromRoute] int id)
        {
            var deleteBook = await _bookService.DeleteBookAsync(id);

            if (!deleteBook.Success)
            {
                return BadRequest(new APIResponse(400, deleteBook.Message));
            }

            return Ok(new { message = deleteBook.Message });
        }

        [HttpGet("book-exists")]
        public async Task<ActionResult<bool>> CheckBookExistByTitleAsync([FromQuery] string bookTitle)
        {
            var response = await _bookService.CheckBookExistByTitleAsync(bookTitle);

            return Ok(response);
        }

        [HttpGet("exists-in-bookstore")]
        public async Task<ActionResult<bool>> CheckBookExistByTitleAsync([FromQuery] string bookTitle, [FromQuery] int bookStoreId)
        {
            var response = await _bookService.CheckBookExistInBookStoreAsync(bookTitle, bookStoreId);

            return Ok(response);
        }

        [HttpGet("check-isbn")]
        public async Task<ActionResult<bool>> CheckBookExistByISBNAsync([FromQuery] string isbn, [FromQuery] string bookTitle)
        {
            var response = await _bookService.CheckBookISBNAsync(isbn, bookTitle);

            return Ok(response);
        }

        [HttpPost("{id:int}/upload-images")]
        public async Task<IActionResult> UploadBookImagesAsync([FromRoute] int id, [FromForm] List<IFormFile> files)
        {
            var uploadBookImages = await _bookService.UploadBookImagesAsync(id, files);
            if (!uploadBookImages.Success)
            {
                return BadRequest(new APIResponse(400, uploadBookImages.Message, uploadBookImages.Errors));
            }
            return Ok(new { message = uploadBookImages.Message });
        }
    }
}
