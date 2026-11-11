using AutoMapper;
using MainHW.Models;
using MainHW.Models.DTO;
using MainHW.Services;
using Microsoft.AspNetCore.Mvc;

namespace MainHW.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly OrderService _orderService;
    private readonly IMapper _mapper;

    public OrderController(OrderService orderService, IMapper mapper)
    {
        _orderService = orderService;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all orders.
    /// </summary>
    /// <returns>An action result containing the collection of order DTOs.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllOrdersAsync()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders);
        return Ok(orderDtos);
    }

    /// <summary>
    /// Adds a new order.
    /// </summary>
    /// <param name="orderDto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> AddOrderAsync([FromBody] OrderDto orderDto)
    {
        var newOrder = _mapper.Map<Order>(orderDto);
        newOrder.DateTime = DateTime.Now;
        var createdOrder = await _orderService.AddOrderAsync(newOrder);
        return Ok(createdOrder);

    }

    /// <summary>
    /// Updates an order's completion status.
    /// </summary>
    /// <param name="id">The order identifier.</param>
    /// <param name="isCompleted">A value indicating whether the order is completed.</param>
    /// <returns>A <see cref="NotFoundResult"/> if the order is not found; otherwise, a <see cref="NoContentResult"/>.</returns>
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateOrderStatusAsync(int id, [FromBody] bool isCompleted)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        if (order == null)
        {
            return NotFound();
        }
        order.IsCompleted = isCompleted;
        var result = await _orderService.UpdateOrderAsync(id, order);
        if (result == null)
        {
            return NotFound();
        }
        return NoContent();
    }


    /// <summary>
    /// Updates the quantity of an order.
    /// </summary>
    /// <param name="id">The identifier of the order to update.</param>
    /// <param name="newQuantity">The new quantity for the order.</param>
    /// <returns>A <see cref="NotFoundResult"/> if the order does not exist; otherwise, a <see cref="NoContentResult"/>.</returns>
    [HttpPut("{id}/quantity")]
    public async Task<IActionResult> UpdateOrderQuantityAsync(int id, [FromBody] int newQuantity)
    {
        var existingOrder = await _orderService.GetOrderByIdAsync(id);
        if (existingOrder == null)
        {
            return NotFound();
        }
        existingOrder.Quantity = newQuantity;
        var result = await _orderService.UpdateOrderAsync(id, existingOrder);
        if (result == null)
        {
            return NotFound();
        }
        return NoContent();
    }

    
}
