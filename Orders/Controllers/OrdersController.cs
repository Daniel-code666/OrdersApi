using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Domain.Entitites.Base;
using Orders.Application.Orders.Dtos;
using Orders.Application.Orders.UseCases.OrdersBusiness.Interfaces;

namespace Orders.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersBusiness _ordersBusiness;

        public OrdersController(IOrdersBusiness ordersBusiness)
        {
            _ordersBusiness = ordersBusiness;
        }

        [HttpPost]
        public async Task<ActionResult<DbActions>> CreateOrder([FromBody] OrdersCreate order)
            => Ok(await _ordersBusiness.CreateOrder(order));

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<OrdersRead?>> GetById(int id)
            => Ok(await _ordersBusiness.GetById(id));

        [HttpPatch("GetAll")]
        public async Task<ActionResult<IEnumerable<OrdersRead>>> GetAll([FromBody] OrdersFilters filters)
            => Ok(await _ordersBusiness.GetAll(filters));
    }
}
