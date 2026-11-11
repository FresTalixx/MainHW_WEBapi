using AutoMapper;
using MainHW.Models;
using MainHW.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MainHW.Services;

/// <summary>
/// Provides operations for retrieving, creating, updating, and deleting orders.
/// </summary>
public class OrderRepository(ApiDbContext context) : IRepository<Order>
{
    public void Add(Order newOrder)
    {
        context.Orders.Add(newOrder);
    }

    public async Task<ICollection<Order>> GetAllAsync()
    {
        return await context.Orders.ToListAsync();
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await context.Orders.FindAsync(id);
    }

    public void Remove(Order order)
    {
        context.Orders.Remove(order);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
