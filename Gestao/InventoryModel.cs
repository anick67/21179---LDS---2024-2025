// Item.cs - Represents an inventory item
public class Item
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; }
    public DateTime LastUpdated { get; set; }
}
 
// InventoryModel.cs - Handles inventory data operations
public class InventoryModel
{
    private List<Item> _items;
    private string _dataFilePath;
 
    // Delegate and event to notify when a new item is added
    public delegate void ItemAddedEventHandler(object sender, Item item); // add to 12-04-2025
    public event ItemAddedEventHandler ItemAdded; // add to 12-04-2025
 
    public InventoryModel(string dataFilePath)
    {
        _dataFilePath = dataFilePath;
        LoadInventory();
    }
    
    // Load inventory from JSON file
    public void LoadInventory()
    {
        if (File.Exists(_dataFilePath))
        {
            //add to 13-04-2025
            try
            {
                string json = File.ReadAllText(_dataFilePath);
                _items = JsonConvert.DeserializeObject<List<Item>>(json);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error loading inventory: Unable to load inventory data. The file may be corrupted.");
                _items = new List<Item>();
            }
 
        }
        else
        {
            _items = new List<Item>();
        }
    }
    
    // Save inventory to JSON file
    public void SaveInventory()
    {
        string json = JsonConvert.SerializeObject(_items, Formatting.Indented);
        File.WriteAllText(_dataFilePath, json);
    }
    
    // CRUD operations
    public List<Item> GetAllItems() => _items;
    
    public Item GetItemById(string id) => _items.FirstOrDefault(i => i.Id == id);
    
    public void AddItem(Item item)
    {
        // Validate that the item name is not empty.(add to 13-04-2024)
        if (string.IsNullOrWhiteSpace(item.Name))
            throw new ArgumentException("Item name cannot be empty.");
 
        // Validate that the item quantity is non-negative.(add to 13-04-2024)
        if (item.Quantity < 0)
            throw new ArgumentOutOfRangeException("Item quantity cannot be negative.");
 
        // Validate that the item price is non-negative.(add to 13-04-2024)
        if (item.Price < 0)
            throw new ArgumentOutOfRangeException("Item price cannot be negative.");
 
        // Validate that the item category is not empty.(add to 13-04-2024)
        if (string.IsNullOrWhiteSpace(item.Category))
            throw new ArgumentException("Item category cannot be empty.");
 
        // Validate that the item ID is unique.(add to 13-04-2024)
        if (_items.Any(i => i.Id == item.Id))
            throw new ArgumentException("Item ID must be unique.");
 
        // Validate that the item description is not empty.(add to 13-04-2024)
        if (string.IsNullOrWhiteSpace(item.Description))
            throw new ArgumentException("Item description cannot be empty.");
 
        item.LastUpdated = DateTime.Now;
        _items.Add(item);
        SaveInventory();
        ItemAdded?.Invoke(this, item); // add to 12-04-2025 (trigger the event)
    }
    
    public void UpdateItem(Item item)
    {
        var existingItem = GetItemById(item.Id);
        if (existingItem != null)
        {
            int index = _items.IndexOf(existingItem);
            item.LastUpdated = DateTime.Now;
            _items[index] = item;
            SaveInventory();
            UpdateItemAdded?.Invoke(this, item); // add to 12-04-2025 (trigger the event)
 
        }
    }
    
    public void DeleteItem(string id)
    {
        var item = GetItemById(id);
        if (item != null)
        {
            item.LastUpdated = DateTime.Now;
            _items.Remove(item);
            SaveInventory();
        }
    }
    
    // Additional methods for filtering, sorting, etc.
    public List<Item> GetItemsByCategory(string category)
    {
        return _items.Where(i => i.Category == category).ToList();
    }
    
    public List<Item> GetLowStockItems(int threshold)
    {
        return _items.Where(i => i.Quantity < threshold).ToList();
    }
}
Fechar