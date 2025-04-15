// InventoryController.cs
public class InventoryController
{
    private readonly InventoryModel _model;
    
    public InventoryController(InventoryModel model)
    {
        _model = model;
        _model.ItemAdded += OnItemAdded; //add to 12-04-2025 (Subscribe to the event)
        _model.UpdateItemAdded += OnItemUpdated; //add to 12-04-2025 (Subscribe to the event)
 
    }
 
    //add to 12-04-2025 (event handler method)
    private void OnItemAdded(object sender, Item item)
    {
        Console.WriteLine($"[EVENTO] New item added: {item.Name}");
    }
 
    //add to 12-04-2025 (event handler method)
    private void OnItemUpdated(object sender, Item item)
    {
        Console.WriteLine($"[EVENTO] Quantity of item updated: {item.Quantity}");
    }
 
    // Controller methods that handle user actions
    public List<Item> GetInventory()
    {
        return _model.GetAllItems();
    }
    
    public bool AddNewItem(string name, string description, int quantity, decimal price, string category)
    {
        try
        {
            // Validate that name must not be empty (add to 13-04-2025)
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Error: Item name cannot be empty.");
                return false;
            }
            // Validate that the item quantity is non-negative (add to 13-04-2025)
            if (quantity < 0)
            {
                Console.WriteLine("Error: Item quantity cannot be negative.");
                return false;
            }
            // Validate that the item price is non-negative (add to 13-04-2025)
            if (price < 0)
            {
                Console.WriteLine("Error: Item price cannot be negative.");
                return false;
            }
            // Validate that the item category is not empty (add to 13-04-2025)
            if (string.IsNullOrWhiteSpace(category))
            {
                Console.WriteLine("Error: Item category cannot be empty.");
                return false;
            }
            // Validate that the item ID is unique (add to 13-04-2025)  
            if (_model.GetAllItems().Any(i => i.Name == name))
            {
                Console.WriteLine("Error: Item with the same name already exists.");
                return false;
            }
            // Validate that the item description is not empty (add to 13-04-2025)
            if (string.IsNullOrWhiteSpace(description))
            {
                Console.WriteLine("Error: Item description cannot be empty.");
                return false;
            }
 
            var item = new Item
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Description = description,
                Quantity = quantity,
                Price = price,
                Category = category
            };
            
            _model.AddItem(item);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    
    public bool UpdateItemQuantity(string id, int newQuantity)
    {
        try
        {
            var item = _model.GetItemById(id);
            if (item != null)
            {
                item.Quantity = newQuantity;
                _model.UpdateItem(item);
                return true;
            }
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }
    
    public bool RemoveItem(string id)
    {
        try
        {
            _model.DeleteItem(id);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    
    public List<Item> SearchInventory(string query)
    {
        return _model.GetAllItems()
            .Where(i => i.Name.Contains(query) || 
                   i.Description.Contains(query) || 
                   i.Category.Contains(query))
            .ToList();
    }
    
    public List<Item> GetLowStockAlert(int threshold = 5)
    {
        return _model.GetLowStockItems(threshold);
    }
}
Fechar