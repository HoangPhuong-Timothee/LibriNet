using AutoMapper;
using Libri.API.DTOs.Request;
using Libri.API.DTOs.Response.Errors;
using Libri.API.DTOs.Response.Inventory;
using Libri.API.Helpers;
using Libri.BAL.Helpers.EntityParams;
using Libri.BAL.Services.Interfaces;
using Libri.DAL.Models.Custom.Pagination;
using Libri.DAL.Models.Xml;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Libri.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UnitOfMeasuresController : ControllerBase
    {
        private readonly IUnitOfMeasureService _uomService;
        private readonly IMapper _mapper;
        public UnitOfMeasuresController(IUnitOfMeasureService uomService, IMapper mapper)
        {
            _mapper = mapper;
            _uomService = uomService;
        }

        [Cached(10000)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UnitOfMeasureDTO>>> GetAllUnitOfMeasuresAsync()
        {
            var unitOfMeasures = await _uomService.GetAllUnitOfMeasuresAsync();

            var response = _mapper.Map<IEnumerable<UnitOfMeasureDTO>>(unitOfMeasures);

            return Ok(response);
        }

        [Cached(600)]
        [HttpGet("admin/unit-of-measures-list")]
        public async Task<ActionResult<Pagination<UnitOfMeasureDTO>>> GetAllUnitOfMeasuresAsync([FromQuery] UnitOfMeasureParams uomParams)
        {
            var result = await _uomService.GetAllUnitOfMeasuresForAdminAsync(uomParams);
            
            var unitOfMeasures = _mapper.Map<IEnumerable<UnitOfMeasureDTO>>(result);

            var totalItems = result.FirstOrDefault()?.TotalCount ?? 0;

            var response = new Pagination<UnitOfMeasureDTO>(uomParams.PageIndex, uomParams.PageSize, totalItems, unitOfMeasures);

            return Ok(response);
        }

        [InvalidateCache("/api/UnitOfMeasures")]
        [HttpPost]
        public async Task<IActionResult> AddUnitOfMeasureAsync([FromBody] ModifyUnitOfMeasureRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var unitOfMeasure = _mapper.Map<MeasureUnit>(request);

            var addUnitOfMeasure = await _uomService.AddNewUnitOfMeasureAsync(unitOfMeasure);

            if (!addUnitOfMeasure.Success)
            {
                return BadRequest(new APIResponse(400, addUnitOfMeasure.Message));
            }

            return StatusCode(201, new { message = addUnitOfMeasure.Message });
        }

        [InvalidateCache("api/UnitOfMeasures")]
        [HttpPost("import-from-file")]
        public async Task<IActionResult> ImportUnitOfMeasuresFromFileAsync([FromForm] IFormFile file)
        {
            var importUnitOfMeasures = await _uomService.ImportUnitOfMeasuresFromFileAsync(file);

            if (!importUnitOfMeasures.Success)
            {
                return BadRequest(new APIResponse(400, importUnitOfMeasures.Message, importUnitOfMeasures.Errors!));
            }

            return StatusCode(201, new { message = importUnitOfMeasures.Message });

        }

        [InvalidateCache("api/UnitOfMeasures")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateUnitOfMeasureAsync([FromRoute] int id, [FromBody] ModifyUnitOfMeasureRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var unitOfMeasure = _mapper.Map<MeasureUnit>(request);

            var updateUnitOfMeasure = await _uomService.UpdateUnitOfMeasureAsync(id, unitOfMeasure);

            if (!updateUnitOfMeasure.Success)
            {
                return BadRequest(new APIResponse(400, updateUnitOfMeasure.Message));
            }

            return Ok(new { message = updateUnitOfMeasure.Message });
        }

        [InvalidateCache("api/UnitOfMeasures")]
        [HttpDelete("soft-delete/{id:int}")]
        public async Task<IActionResult> DeleteGenreAsync([FromRoute] int id)
        {
            var deleteUnitOfMeasure = await _uomService.DeleteUnitOfMeasureByIdAsync(id);

            if (!deleteUnitOfMeasure.Success)
            {
                return BadRequest(new APIResponse(400, deleteUnitOfMeasure.Message));
            }

            return Ok(new { message = deleteUnitOfMeasure.Message });
        }

        [HttpGet("unit-of-measure-exists")]
        public async Task<ActionResult<bool>> CheckUnitOfMeasureExistByNameAsync([FromQuery] string name)
        {
            var response = await _uomService.CheckUnitOfMeasureExistByNameAsync(name);

            return Ok(response);
        }
    }
}
