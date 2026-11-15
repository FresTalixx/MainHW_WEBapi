using AutoMapper;
using MainHW.Models;
using MainHW.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MainHW.Repositories;

/// <summary>
/// Provides operations for retrieving, creating, updating, and deleting orders.
/// </summary>
public class OrderRepository(ApiDbContext context) : BaseRepository<Order>(context)
{
}
