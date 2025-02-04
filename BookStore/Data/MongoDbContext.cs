using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

public class MongoDbDataContext
{
    private readonly IMongoCollection<Cart> _cartCollection;
    private readonly IMongoCollection<CartItem> _cartItemCollection;
    private readonly IServiceProvider _serviceProvider;
    public MongoDbDataContext(string connectionString, string databaseName, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);

        _cartCollection = database.GetCollection<Cart>("carts");
        _cartItemCollection = database.GetCollection<CartItem>("cartItems");

        // Ensure TTL index is created
        var indexKeysDefinition = Builders<Cart>.IndexKeys.Ascending(c => c.CreatedAt);
        var indexModel = new CreateIndexModel<Cart>(
            indexKeysDefinition,
            new CreateIndexOptions { ExpireAfter = TimeSpan.FromMinutes(30) } // TTL set to 30 minutes
        );
        _cartCollection.Indexes.CreateOne(indexModel);
    }


    public async Task AddToCartAsync(string userId, string productId)
    {
        using var scope = _serviceProvider.CreateScope(); // Create a new scope
        var context = scope.ServiceProvider.GetRequiredService<DataContext>(); // Get a fresh DbContext

        var cart = await GetCartAsync(userId);
        if (cart == null)
        {
            cart = new Cart { UserId = userId, CreatedAt = DateTime.UtcNow };
            await _cartCollection.InsertOneAsync(cart);
        }

        var cartItem = await _cartItemCollection.Find(i => i.CartId == cart.Id && i.ItemId == productId).FirstOrDefaultAsync();
        if (cartItem != null)
        {
            var update = Builders<CartItem>.Update.Inc(i => i.Count, 1);
            await _cartItemCollection.UpdateOneAsync(i => i.Id == cartItem.Id, update);
        }
        else
        {
            var book = await context.Books.FindAsync(Guid.Parse(productId));
            cartItem = new CartItem { CartId = cart.Id, ItemId = productId, Count = 1, Price = book.Price };
            await _cartItemCollection.InsertOneAsync(cartItem);
        }
    }

    // Get the entire cart for a user
    public async Task<Cart> GetCartAsync(string userId)
    {
        return await _cartCollection.Find(c => c.UserId == userId).FirstOrDefaultAsync();
    }

    // Get all items in the cart for a user
    public async Task<List<CartItem>> GetCartItemsAsync(string userId)
    {
        var cart = await GetCartAsync(userId);
        if (cart == null) return new List<CartItem>();

        return await _cartItemCollection.Find(i => i.CartId == cart.Id).ToListAsync();
    }

    // Remove a single quantity of an item from the cart
    public async Task RemoveCartItemAsync(string userId, string productId)
    {
        var cart = await GetCartAsync(userId);
        if (cart == null) return;

        var cartItem = await _cartItemCollection.Find(i => i.CartId == cart.Id && i.ItemId == productId).FirstOrDefaultAsync();
        if (cartItem == null) return;

        if (cartItem.Count > 1)
        {
            // Decrement the count if there's more than one item
            var update = Builders<CartItem>.Update.Inc(i => i.Count, -1);
            await _cartItemCollection.UpdateOneAsync(i => i.Id == cartItem.Id, update);
        }
        else
        {
            // Remove the item if the count is 1 or less
            await _cartItemCollection.DeleteOneAsync(i => i.Id == cartItem.Id);
        }
    }

    // Completely remove an item from the cart
    public async Task RemoveFromCartAsync(string userId, string productId)
    {
        var cart = await GetCartAsync(userId);
        if (cart == null) return;

        await _cartItemCollection.DeleteOneAsync(i => i.CartId == cart.Id && i.ItemId == productId);
    }

    // Clear the entire cart
    public async Task ClearCartAsync(string userId)
    {
        var cart = await GetCartAsync(userId);
        if (cart == null) return;

        await _cartItemCollection.DeleteManyAsync(i => i.CartId == cart.Id);
        await _cartCollection.DeleteOneAsync(c => c.Id == cart.Id);
    }

    // Get total count of items in the cart
    public async Task<int> GetCartCountAsync(string userId)
    {
        var cart = await GetCartAsync(userId);
        if (cart == null) return 0;

        var cartItems = await _cartItemCollection.Find(i => i.CartId == cart.Id).ToListAsync();
        return cartItems.Sum(i => i.Count);
    }
}

// Define the Cart model
public class Cart
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string UserId { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Define the CartItem model
public class CartItem
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string CartId { get; set; } // Reference to the Cart
    public string ItemId { get; set; }
    public double Price { get; set; }
    public int Count { get; set; }
}