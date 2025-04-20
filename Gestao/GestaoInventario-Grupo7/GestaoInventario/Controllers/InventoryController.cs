using System;
using System.Collections.Generic;
using System.Linq;
using GestaoInventario.Models;

namespace GestaoInventario.Controllers
{
    /// <summary>
    /// Controlador responsável pela ligação entre o modelo e a interface gráfica.
    /// Processa ações do utilizador e comunica com o modelo de dados.
    /// </summary>
    public class InventoryController
    {
        private readonly InventoryModel _model;

        /// <summary>
        /// Inicializa o controlador e subscreve eventos do modelo.
        /// </summary>
        public InventoryController(InventoryModel model)
        {
            _model = model;
            _model.ItemAdded += OnItemAdded;
            _model.ItemUpdated += OnItemUpdated;
        }

        /// <summary>
        /// Evento executado quando um item é adicionado.
        /// </summary>
        private void OnItemAdded(object sender, Item item)
        {
            Console.WriteLine($"[EVENTO] Novo item adicionado: {item.Name}");
        }

        /// <summary>
        /// Evento executado quando um item é atualizado.
        /// </summary>
        private void OnItemUpdated(object sender, Item item)
        {
            Console.WriteLine($"[EVENTO] Quantidade do item atualizada: {item.Quantity}");
        }

        /// <summary>
        /// Devolve a lista completa de itens no inventário.
        /// </summary>
        public List<Item> GetInventory() => _model.GetAllItems();

        /// <summary>
        /// Devolve um item específico com base no ID.
        /// </summary>
        public Item? GetItemById(string id) => _model.GetItemById(id);

        /// <summary>
        /// Adiciona um novo item ao inventário com validações.
        /// </summary>
        public bool AddNewItem(string name, string description, int quantity, decimal price, string category)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(description) ||
                    string.IsNullOrWhiteSpace(category) || quantity < 0 || price < 0)
                    return false;

                // Garante que o ID é único
                string newId;
                var existingIds = _model.GetAllItems().Select(i => i.Id).ToHashSet();

                do
                {
                    newId = new Random().Next(10000, 99999).ToString();
                } while (existingIds.Contains(newId));

                var item = new Item
                {
                    Id = newId,
                    Name = name,
                    Description = description,
                    Quantity = quantity,
                    Price = price,
                    Category = category
                };

                _model.AddItem(item);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Atualiza a quantidade de um item com base no ID.
        /// </summary>
        public bool UpdateItemQuantity(string id, int newQuantity)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return false;

                var item = _model.GetItemById(id);
                if (item == null)
                    return false;

                item.Quantity = newQuantity;
                _model.UpdateItem(item);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Remove um item do inventário.
        /// </summary>
        public bool DeleteItem(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return false;

                _model.DeleteItem(id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Pesquisa por itens que contenham o texto indicado no nome, descrição ou categoria.
        /// </summary>
        public List<Item> SearchInventory(string query) =>
            _model.GetAllItems().Where(i =>
                (i.Name != null && i.Name.Contains(query, StringComparison.OrdinalIgnoreCase)) ||
                (i.Description != null && i.Description.Contains(query, StringComparison.OrdinalIgnoreCase)) ||
                (i.Category != null && i.Category.Contains(query, StringComparison.OrdinalIgnoreCase))).ToList();

        /// <summary>
        /// Devolve a lista de itens com stock abaixo do limite definido.
        /// </summary>
        public List<Item> GetLowStockAlert(int threshold = 5) => _model.GetLowStockItems(threshold);
    }
}
