using System.Collections.Generic;
using GestaoInventario.Models;

namespace GestaoInventario.Views
{
    public interface IInventoryView
    {
        void DisplayInventory(List<Item> items);
        void DisplayItemDetails(Item item);
        void ShowMessage(string message);
        void RefreshView();
    }
}