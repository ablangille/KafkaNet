using Microsoft.AspNetCore.Mvc;
using KafkaDocker.Data.Models;
using KafkaDocker.Data.Repository;
using KafkaDocker.Publisher.Request;

namespace KafkaDocker.Publisher.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderRequest _handler;
        private readonly IOrderRepository _repository;

        public OrderController(
            ILogger<OrderController> logger,
            IOrderRequest handler,
            IOrderRepository repository
        )
        {
            _logger = logger;
            _handler = handler;
            _repository = repository;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Order))]
        public async Task<ActionResult<Order>> SendOrder(OrderRequest orderRequest)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                ProductId = orderRequest.ProductId,
                CustomerId = orderRequest.CustomerId,
                Quantity = orderRequest.Quantity,
                Status = "Sent"
            };

            var response = await _handler.SendOrderRequest(order);

            return StatusCode(response.Status, response.Message != null ? response.Message : order);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Order>))]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
        {
            try
            {
                var users = await Task.FromResult(_repository.GetOrders());

                return Ok(users);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, $"Error occurred: {ex.Message}");
                return StatusCode(
                    StatusCodes.Status503ServiceUnavailable,
                    "Connection to DB failed"
                );
            }
        }
    }
}
