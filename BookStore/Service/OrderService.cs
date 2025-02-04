using AutoMapper;
using AutoMapper.QueryableExtensions;
using black_follow.Entity;
using BookStore.Controllers;
using BookStore.Data.Dto.Order;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Services;

public interface IOrderService
{
    Task<(OrderDto? data, string? message)> SendCartOrder(OrderCartForm form, Guid userId);
    Task<(OrderDto? data, string? message)> SendOrder(OrderForm form, Guid userId);
    Task<(List<OrderDto>? data, int totalCount, string? message)> GetAll(OrderFilter filter, Guid userId, string role);
    Task<(bool? state, string? message)> ChangeOrderStatus(ChangeOrderStateForm form);
    Task<(bool? state, string? message)> CancelOrder(Guid orderId, Guid userId);
}

public class OrderService : IOrderService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly MongoDbDataContext _mongoDbContext;


    public OrderService(IMapper mapper, DataContext context, MongoDbDataContext mongoDbContext)
    {
        _mapper = mapper;
        _context = context;
        _mongoDbContext = mongoDbContext;
    }

    public async Task<(OrderDto? data, string? message)> SendCartOrder(OrderCartForm form,  Guid userId)
    {
        var cart = await _mongoDbContext.GetCartAsync(userId.ToString());
        var cartItems = await _mongoDbContext.GetCartItemsAsync(userId.ToString());
        if (cart == null)
        {
            return (null, "There is no items in cart");
        }

        var _orderItems = cartItems.Select(ci => new OrderItem()
        {
            BookId = Guid.Parse(ci.ItemId),
            Count = ci.Count,
            Price = ci.Price
        }).ToList();
        var _order = new Order()
        {
            Notes = form.Notes,
            UserId = userId,
            Items = _orderItems
        };

        // Save the order to the database
        var _savedOrder = await _context.Orders.AddAsync(_order);
        await _context.SaveChangesAsync();
       await  _mongoDbContext.ClearCartAsync(userId.ToString());
        // Return the mapped order DTO
        return (_mapper.Map<OrderDto>(_savedOrder.Entity), null);

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
            
        }
        else
        {
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

    public async Task<(bool? state, string? message)> ChangeOrderStatus(ChangeOrderStateForm form)
    {
        var _order = await _context.Orders.FindAsync(form.OrderId);
        if (_order is null)
        {
            return (false, "Order not Found");
        }

        if (_order.Status is OrderStatus.CANCELLED)
        {
            return (false, "The Order Was Canceled by the user");

        }

        _order.Status = form.State;
        _context.Orders.Update(_order);
        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool? state, string? message)> CancelOrder( Guid orderId, Guid userId)
    {
        
        var _order = await _context.Orders.FindAsync(orderId);
        if (_order is null)
        {
            return (false, "Order not Found");
        }

        if (_order.UserId != userId )
        {
            return (false, "it's not your order");
        }

        if (_order.Status!= OrderStatus.PENDING)
        {
            return (false, "you can only cancel pending orders");
        }
        _order.Status = OrderStatus.CANCELLED;
        _context.Orders.Update(_order);
        await _context.SaveChangesAsync();
        return (true, null);
    }
   
}