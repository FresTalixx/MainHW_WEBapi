using AutoMapper;
using MainHW.Models;

namespace MainHW.Services;

public class OrderService
{
    private readonly OrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public OrderService(OrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<ICollection<Order>> GetAllOrdersAsync()
    {
        return await _orderRepository.GetAllAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        return await _orderRepository.GetByIdAsync(id);
    }

    public async Task<Order> AddOrderAsync(Order newOrder)
    {
        _orderRepository.Add(newOrder);
        await _orderRepository.SaveChangesAsync();
        return newOrder;
    }

    public async Task<Order?> UpdateOrderAsync(int id, Order updatedOrder)
    {
        var existingOrder = await _orderRepository.GetByIdAsync(id);
        if (existingOrder == null)
        {
            return null;
        }
        _mapper.Map(updatedOrder, existingOrder);
        await _orderRepository.SaveChangesAsync();
        return existingOrder;
    }

    public async Task<bool> DeleteOrderAsync(int id)
    {
        var existingOrder = await _orderRepository.GetByIdAsync(id);
        if (existingOrder == null)
        {
            return false;
        }
        _orderRepository.Remove(existingOrder);
        await _orderRepository.SaveChangesAsync();
        return true;
    }

}
