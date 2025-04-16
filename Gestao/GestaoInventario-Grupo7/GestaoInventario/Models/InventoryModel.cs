using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace GestaoInventario.Models
{
    /// <summary>
    /// Modelo responsável por gerir os dados do inventário,
    /// incluindo operações CRUD e persistência em ficheiro JSON.
    /// </summary>
    public class InventoryModel
    {
        private List<Item> _items = new List<Item>();
        private string _dataFilePath;

        /// <summary>
        /// Evento acionado quando um item é adicionado.
        /// </summary>
        public delegate void ItemAddedEventHandler(object sender, Item item);
        public event ItemAddedEventHandler? ItemAdded;

        /// <summary>
        /// Evento acionado quando um item é atualizado.
        /// </summary>
        public delegate void ItemUpdatedEventHandler(object sender, Item item);
        public event ItemUpdatedEventHandler? ItemUpdated;

        /// <summary>
        /// Construtor que define o caminho do ficheiro e carrega o inventário.
        /// </summary>
        public InventoryModel(string dataFilePath)
        {
            _dataFilePath = dataFilePath;
            LoadInventory();
        }

        /// <summary>
        /// Carrega os dados do inventário a partir de um ficheiro JSON.
        /// </summary>
        public void LoadInventory()
        {
            if (File.Exists(_dataFilePath))
            {
                try
                {
                    string json = File.ReadAllText(_dataFilePath);
                    _items = JsonConvert.DeserializeObject<List<Item>>(json) ?? new List<Item>();
                }
                catch (JsonException)
                {
                    Console.WriteLine("Erro ao carregar inventário: ficheiro pode estar corrompido.");
                    _items = new List<Item>();
                }
            }
            else
            {
                _items = new List<Item>();
            }
        }

        /// <summary>
        /// Guarda os dados do inventário no ficheiro JSON.
        /// </summary>
        public void SaveInventory()
        {
            string json = JsonConvert.SerializeObject(_items, Formatting.Indented);
            File.WriteAllText(_dataFilePath, json);
        }

        /// <summary>
        /// Devolve todos os itens do inventário.
        /// </summary>
        public List<Item> GetAllItems() => _items;

        /// <summary>
        /// Procura um item pelo seu ID.
        /// </summary>
        public Item? GetItemById(string id) => _items.FirstOrDefault(i => i.Id == id);

        /// <summary>
        /// Adiciona um novo item ao inventário, com validações.
        /// </summary>
        public void AddItem(Item item)
        {
            if (string.IsNullOrWhiteSpace(item.Name))
                throw new ArgumentException("Nome do item não pode estar vazio.");
            if (item.Quantity < 0)
                throw new ArgumentOutOfRangeException("Quantidade não pode ser negativa.");
            if (item.Price < 0)
                throw new ArgumentOutOfRangeException("Preço não pode ser negativo.");
            if (string.IsNullOrWhiteSpace(item.Category))
                throw new ArgumentException("Categoria do item não pode estar vazia.");
            if (_items.Any(i => i.Id == item.Id))
                throw new ArgumentException("ID do item deve ser único.");
            if (string.IsNullOrWhiteSpace(item.Description))
                throw new ArgumentException("Descrição do item não pode estar vazia.");

            item.LastUpdated = DateTime.Now;
            _items.Add(item);
            SaveInventory();
            ItemAdded?.Invoke(this, item);
        }

        /// <summary>
        /// Atualiza um item existente no inventário.
        /// </summary>
        public void UpdateItem(Item item)
        {
            if (item.Id == null)
                return; // Evita referência nula

            var existingItem = GetItemById(item.Id);
            if (existingItem != null)
            {
                int index = _items.IndexOf(existingItem);
                item.LastUpdated = DateTime.Now;
                _items[index] = item;
                SaveInventory();
                ItemUpdated?.Invoke(this, item);
            }
        }

        /// <summary>
        /// Remove um item do inventário com base no ID.
        /// </summary>
        public void DeleteItem(string id)
        {
            var item = GetItemById(id);
            if (item != null)
            {
                _items.Remove(item);
                SaveInventory();
            }
        }

        /// <summary>
        /// Devolve todos os itens de uma determinada categoria.
        /// </summary>
        public List<Item> GetItemsByCategory(string category) =>
            _items.Where(i => i.Category == category).ToList();

        /// <summary>
        /// Devolve os itens com quantidade inferior ao limite definido.
        /// </summary>
        public List<Item> GetLowStockItems(int threshold) =>
            _items.Where(i => i.Quantity < threshold).ToList();
    }
}
