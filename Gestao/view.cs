// IInventoryView.cs - Interface for the view
public interface IInventoryView
{
    void DisplayInventory(List<Item> items);
    void DisplayItemDetails(Item item);
    void ShowMessage(string message);
    void RefreshView();
}
 
// Example implementation for a WinForms app
public class InventoryFormView : Form, IInventoryView
{
    private InventoryController _controller;
    private DataGridView _itemsGrid;
    // Other UI controls...
    
    public InventoryFormView(InventoryController controller)
    {
        _controller = controller;
        InitializeComponents();
    }
    
    private void InitializeComponents()
    {
        // Setup form controls, buttons, grid, etc.
        // Attach event handlers
    }
 
    lowStockButton.Click += LowStockButton_Click; // add to 13-04-2025
    
    public void DisplayInventory(List<Item> items)
    {
        _itemsGrid.DataSource = null;
        _itemsGrid.DataSource = items;
    }
    
    public void DisplayItemDetails(Item item)
    {
        // Show item details in a form or panel
    }
    
    public void ShowMessage(string message)
    {
        MessageBox.Show(message);
    }
    
    public void RefreshView()
    {
        DisplayInventory(_controller.GetInventory());
    }
 
    // Validate the input fields in the UI. (add to 13-04-2025)
    private bool ValidateInputs()
    {
        // Check if the item name is empty.
        if (string.IsNullOrWhiteSpace(nameTextBox.Text))
        {
            ShowMessage("Item name cannot be empty.");
            return false;
        }
        // Check if the quantity is negative.
        if (quantityNumeric.Value < 0)
        {
            ShowMessage("Item quantity cannot be negative.");
            return false;
        }
        // Check if the price is negative.
        if (priceNumeric.Value < 0)
        {
            ShowMessage("Item price cannot be negative.");
            return false;
        }
        // Check if the category is empty.
        if (string.IsNullOrWhiteSpace(categoryComboBox.Text))
        {
            ShowMessage("Item category cannot be empty.");
            return false;
        }
        // Check if the ID is unique (if applicable).
        // Assuming we have a method to check for uniqueness
        if (_controller.GetItemById(idTextBox.Text) != null)
        {
            ShowMessage("Item ID must be unique.");
            return false;
        }
        // Check if the description is empty.
        if (string.IsNullOrWhiteSpace(descriptionTextBox.Text))
        {
            ShowMessage("Item description cannot be empty.");
            return false;
        }
        return true;
    }
 
    // Event handlers for user interactions
    private void AddButton_Click(object sender, EventArgs e)
    {
        // Validate inputs before adding a new item (add to 13-04-2025)
        if (!ValidateInputs())
        {
            return;
        }
 
        // Collect input from UI
        string name = nameTextBox.Text;
        string description = descriptionTextBox.Text;
        int quantity = (int)quantityNumeric.Value;
        decimal price = priceNumeric.Value;
        string category = categoryComboBox.Text;
        
        try //add to 13-04-2025
        {
            if (_controller.AddNewItem(name, description, quantity, price, category))
            {
                ShowMessage("Item added successfully");
                RefreshView();
            }
            else
            {
                ShowMessage("Error adding item");
            }
        }
        catch (Exception ex)
        {
            ShowMessage($"Error adding item: {ex.Message}");
            return;
        }
 
    }
 
    //Search button event handler (add to 13-04-2025)
    private void SearchButton_Click(object sender, EventArgs e)
    {
        // Retrieve the search query from the searchTextBox
        string query = searchTextBox.Text.Trim();
 
        // Call the SearchInventory method from the controller
        List<Item> searchResults = _controller.SearchInventory(query);
 
        // Check if any items were found and update the UI accordingly
        if (searchResults != null && searchResults.Count > 0)
        {
            // Update the DataGridView with the search results
            DisplayInventory(searchResults);
            ShowMessage($"Search completed. {searchResults.Count} item(s) found.");
        }
        else
        {
            // Display an informative message if no items match the search criteria
            ShowMessage("No items found matching the search criteria.");
        }
    }
 
    // Indicate the low stock inventory (add to 13-04-2025)
    private void LowStockButton_Click(object sender, EventArgs e)
    {
        // Define the threshold. It can be obtained from a control
        int threshold = 5;
 
        // Call the controller method to get the list of items with low stock
        List<Item> lowStockItems = _controller.GetLowStockAlert(threshold);
 
        // Update the user interface with the low stock items
        if (lowStockItems != null && lowStockItems.Count > 0)
        {
            DisplayInventory(lowStockItems);
            ShowMessage($"Low stock alert: {lowStockItems.Count} item(s) found.");
        }
        else
        {
            ShowMessage("No low stock items found.");
        }
    }
 
    // Update item (add to 13-04-2025)
    private void UpdateButton_Click(object sender, EventArgs e)
    {
        // Collect input from UI
        string id = idTextBox.Text;
        int newQuantity = (int)quantityNumeric.Value;
        if (_controller.UpdateItemQuantity(id, newQuantity))
        {
            ShowMessage("Item updated successfully");
            RefreshView();
        }
        else
        {
            ShowMessage("Error updating item");
        }
    }
 
    // Remove item (add to 13-04-2025)
    private void DeleteButton_Click(object sender, EventArgs e)
    {
        // Check if there is at least one selected row in the DataGridView displaying the items.
        if (_itemsGrid.SelectedRows.Count > 0)
        {
            // Retrieve the ID of the selected item.
            // Assuming that the "Id" column is available in the DataGridView.
            string selectedItemId = _itemsGrid.SelectedRows[0].Cells["Id"].Value.ToString();
            
            // Call the controller method to delete the item.
            if (_controller.DeleteItem(id))
            {
                // Refresh the view to reflect the changes.
                ShowMessage("Item deleted successfully");
                RefreshView();
            }
            else
            {
                // Inform the user if there was an error deleting the item.
                ShowMessage("Error deleting item");
            }
            else
            {
                // Inform the user to select an item if no row was selected.
                ShowMessage("Please select an item to remove.");
            }
    }
 
 
 
 
    // Other event handlers for update, delete, search, etc.
}
Fechar