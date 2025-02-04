using BookStore.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

[ApiController]
[Route("api/cart")]
public class CartController : BaseController
{
    private readonly MongoDbDataContext _mongoDbContext;

    public CartController(MongoDbDataContext mongoDbContext)
    {
        _mongoDbContext = mongoDbContext;
    }

    // Add an item to the cart (increments count if item exists)
    [HttpPost("add/{productId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> AddToCart(string productId)
    {
        await _mongoDbContext.AddToCartAsync(Id.ToString(), productId);
        var cart = await _mongoDbContext.GetCartAsync(Id.ToString());
        var cartItems = await _mongoDbContext.GetCartItemsAsync(Id.ToString());
        return Ok(new { Message = "Product added to cart", Cart = cart, Items = cartItems });
    }

    // Get the user's cart and its items
    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetCart()
    {
        var cart = await _mongoDbContext.GetCartAsync(Id.ToString());
        var cartItems = await _mongoDbContext.GetCartItemsAsync(Id.ToString());

        if (cart == null)
        {
            cart = new Cart { UserId = Id.ToString(), CreatedAt = DateTime.UtcNow };
        }

        return Ok(new { Cart = cart, Items = cartItems });
    }

    // Remove a single quantity of an item from the cart
    [HttpDelete("remove/{productId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> RemoveCartItem(string productId)
    {
        await _mongoDbContext.RemoveCartItemAsync(Id.ToString(), productId);
        var cart = await _mongoDbContext.GetCartAsync(Id.ToString());
        var cartItems = await _mongoDbContext.GetCartItemsAsync(Id.ToString());
        return Ok(new { Message = "Product quantity reduced", Cart = cart, Items = cartItems });
    }

    // Completely remove an item from the cart
    [HttpDelete("remove-all/{productId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> RemoveFromCart(string productId)
    {
        await _mongoDbContext.RemoveFromCartAsync(Id.ToString(), productId);
        var cart = await _mongoDbContext.GetCartAsync(Id.ToString());
        var cartItems = await _mongoDbContext.GetCartItemsAsync(Id.ToString());
        return Ok(new { Message = "Product removed from cart", Cart = cart, Items = cartItems });
    }

    // Clear the entire cart
    [HttpDelete("clear")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> ClearCart()
    {
        await _mongoDbContext.ClearCartAsync(Id.ToString());
        return Ok(new { Message = "Cart cleared", Cart = new Cart { UserId = Id.ToString(), CreatedAt = DateTime.UtcNow }, Items = new List<CartItem>() });
    }

    // Get total count of items in the cart
    [HttpGet("count")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetCartCount()
    {
        var count = await _mongoDbContext.GetCartCountAsync(Id.ToString());
        return Ok(new { Count = count });
    }
}