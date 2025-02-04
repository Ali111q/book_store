using AutoMapper;
using AutoMapper.QueryableExtensions;
using black_follow.Entity;
using BookStore.Controllers;
using BookStore.Data.Dto.Order;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Services;

public interface IOrderService
{
    Task<(OrderDto? data, string? message)> SendOrder(OrderForm form, Guid userId);
    Task<(List<OrderDto>? data, int totalCount, string? message)> GetAll(OrderFilter filter, Guid userId, string role);
}

public class OrderService : IOrderService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public OrderService(IMapper mapper, DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<(OrderDto? data, string? message)> SendOrder(OrderForm form, Guid userId)
    {
        // Check for duplicate BookId or Count == 0
        var duplicateBookIds = form.Items.GroupBy(i => i.BookId)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        var invalidItems = form.Items.Where(i => i.Count <= 0).ToList();

        if (duplicateBookIds.Any())
        {
            return (null, $"Invalid order: Duplicate books found ({duplicateBookIds.Count}).");
        }

        if (invalidItems.Any())
        {
            return (null, $"Invalid order: {invalidItems.Count} items have Count = 0.");
        }

        // Extract unique book IDs
        var bookIds = form.Items.Select(i => i.BookId).ToList();

        // Fetch books from the database
        var _items = await _context.Books.Where(b => bookIds.Contains(b.Id)).ToListAsync();

        // Check if all requested books exist in the database
        var missingBooks = bookIds.Except(_items.Select(b => b.Id)).ToList();
        if (missingBooks.Any())
        {
            return (null, $"Invalid order: {missingBooks.Count} items not found in the database.");
        }

        // Map order items
        var _orderItems = form.Items.Select(i => new OrderItem()
        {
            BookId = i.BookId,
            Count = i.Count,
            Price = _items.First(b => b.Id == i.BookId).Price
        }).ToList();

        // Create the order
        var _order = new Order()
        {
            Notes = form.Notes,
            UserId = userId,
            Items = _orderItems
        };

        // Save the order to the database
        var _savedOrder = await _context.Orders.AddAsync(_order);
        await _context.SaveChangesAsync();

        // Return the mapped order DTO
        return (_mapper.Map<OrderDto>(_savedOrder.Entity), null);
    }

    public async Task<(List<OrderDto>? data, int totalCount, string? message)> GetAll(OrderFilter filter, Guid userId, string role)
    {
        var query = _context.Orders.AsQueryable();
        if (role == "User")
        {
            query = query.Where(o => o.UserId == userId);
            if (filter.UserId is not null)
            {
                query = query.Where(o => o.UserId == filter.UserId);
            }
        }

        if (filter.OrderStatus is not null)
        {
            query = query.Where(o=>o.Status == filter.OrderStatus);
        }
        var _count = query.Count();
        var _orders =  await query.OrderBy(b => b.Id)
            .Skip(filter.PageSize * (filter.Page - 1))
            .Take(filter.PageSize).ProjectTo<OrderDto>(_mapper.ConfigurationProvider).ToListAsync();
        return (_orders, _count,  null);
    }
}