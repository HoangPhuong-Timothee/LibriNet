using AutoMapper;
using Libri.API.DTOs.Response.Basket;
using Libri.BAL.Services.Interfaces;
using Libri.DAL.Models.Custom.CustomBasket;
using Microsoft.AspNetCore.Mvc;

namespace Libri.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketService _basketService;
        private readonly IMapper _mapper;
        public BasketsController(IMapper mapper, IBasketService basketService)
        {
            _basketService = basketService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<BasketDTO>> GetBasketByKeyAsync([FromQuery] string id)
        {
            var basket = await _basketService.GetBasketByKeyAsync(id);
            
            if (basket == null)
            {
                return Ok(new Basket(id));
            }

            var response = _mapper.Map<BasketDTO>(basket);

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<BasketDTO>> SetBasketAsync([FromBody] BasketDTO request)
        {
            var basket = _mapper.Map<Basket>(request);

            var basetToSet = await _basketService.SetBasketAsync(basket);

            var response = _mapper.Map<BasketDTO>(basetToSet);

            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBasketByKeyAsync([FromQuery] string id)
        {
            await _basketService.DeleteBasketByKeyAsync(id);

            return Ok(new { message = $"Xóa giỏ hàng: {id} thành công" });
        }
    }
}