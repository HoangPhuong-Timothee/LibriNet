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
    public class PublishersController : ControllerBase
    {
        private readonly IPublisherService _publisherService;
        private readonly IMapper _mapper;
        public PublishersController(IMapper mapper, IPublisherService publisherService)
        {
            _mapper = mapper;
            _publisherService = publisherService;
        }

        [Cached(10000)]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PublisherDTO>>> GetAllPublishersAsync()
        {
            var publishers = await _publisherService.GetAllPublishersAsync();
            
            var response = _mapper.Map<List<PublisherDTO>>(publishers);
            
            return Ok(response);
        }
        
        [Cached(600)]
        [HttpGet("admin/publishers-list")]
        public async Task<ActionResult<Pagination<PublisherDTO>>> GetAllPublishersForAdminAsync([FromQuery] PublisherParams publisherParams)
        {
            var result = await _publisherService.GetAllPublishersForAdminAsync(publisherParams);
            
            var publishers = _mapper.Map<IEnumerable<PublisherDTO>>(result);
            
            var totalItems = result.FirstOrDefault()?.TotalCount ?? 0;
            
            var response = new Pagination<PublisherDTO>(publisherParams.PageIndex, publisherParams.PageSize, totalItems, publishers);
            
            return Ok(response);
        }

        [InvalidateCache("api/Publishers")]
        [HttpPost]
        public async Task<ActionResult> AddPublisherAsync([FromBody] ModifyPublisherRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var publisher = _mapper.Map<Publisher>(request);
            
            var addPublisher = await _publisherService.AddNewPublisherAsync(publisher);
            
            if (!addPublisher.Success) 
            {
                return BadRequest(new APIResponse(400, addPublisher.Message));
            
            }
            return StatusCode(201, new { message = addPublisher.Message });
        }

        [InvalidateCache("api/Publishers")]
        [HttpPost("import-from-file")]
        public async Task<ActionResult> ImportPublishersFromFileAsync([FromForm] IFormFile file)
        {
            var importPublishers = await _publisherService.ImportPublishersFromFileAsync(file);
            
            if (!importPublishers.Success) 
            {
                return BadRequest(new APIResponse(400, importPublishers.Message, importPublishers.Errors));
            }

            return StatusCode(201, new { message = importPublishers.Message });
        }

        [InvalidateCache("api/Publishers")]
        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdatePublisherAsync([FromRoute] int id, [FromBody] ModifyPublisherRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var publisher = _mapper.Map<Publisher>(request);
            
            var updatePublisher = await _publisherService.UpdatePublisherAsync(id, publisher);

            if (!updatePublisher.Success)
            {
                return BadRequest(new APIResponse(400, updatePublisher.Message));
            }
                
            return Ok(new { message = updatePublisher.Message });
        }

        [InvalidateCache("api/Publishers")]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeletePublisherById([FromRoute] int id)
        {
            var publisherToDelete = await _publisherService.DeletePublisherByIdAsync(id);
            
            if (!publisherToDelete.Success) 
            {
                return BadRequest(new APIResponse(400, publisherToDelete.Message));
            }

            return Ok(new { message = publisherToDelete.Message });
        }

        [HttpGet("publisher-exists")]
        public async Task<ActionResult<bool>> CheckPublisherExistByNameAsync([FromQuery] string name)
        {
            var response = await _publisherService.CheckPublisherExistByNameAsync(name);
            
            return Ok(response);
        }
    }
}
