using Libri.API.DTOs.Response;
using Libri.API.DTOs.Response.Errors;
using Libri.DAL.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Libri.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugsController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        public BugsController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet("not-found")]
        public ActionResult GetNotFoundRequest()
        {
            return NotFound(new APIResponse(404));
        }

        [HttpGet("server-error")]
        public IActionResult GetServerError()
        {
            var x = _uow.Authors.GetByIdAsync(100000);
            var xDTO = x.ToString();
            
            return Ok();
        }

        [HttpGet("bad-request")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new APIResponse(400));
        }
    }
}
