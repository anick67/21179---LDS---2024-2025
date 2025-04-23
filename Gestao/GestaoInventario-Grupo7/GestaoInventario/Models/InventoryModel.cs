using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace GestaoInventario.Models
{

    // Modelo responsável por gerir os dados do inventário,
    // incluindo operações CRUD e persistência em ficheiro JSON
    public class InventoryModel
    {
        private List<Item> _items = new List<Item>();
        private string _dataFilePath;

 
        // Evento acionado quando um item é adicionado
        public delegate void ItemAddedEventHandler(object sender, Item item);
        public event ItemAddedEventHandler? ItemAdded;

  
        // Evento acionado quando um item é atualizado
        public delegate void ItemUpdatedEventHandler(object sender, Item item);
        public event ItemUpdatedEventHandler? ItemUpdated;

  
        /// Construtor que define o caminho do ficheiro e carrega o inventário
        public InventoryModel(string dataFilePath)
        {
            _dataFilePath = dataFilePath;
            LoadInventory();
        }

  
        // Carrega os dados do inventário a partir de um ficheiro JSON
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

      
        // Guarda os dados do inventário no ficheiro JSON
        public void SaveInventory()
        {
            string json = JsonConvert.SerializeObject(_items, Formatting.Indented);
            File.WriteAllText(_dataFilePath, json);
        }

     
        // Devolve todos os itens do inventário
        public List<Item> GetAllItems() => _items;

    
        // Procura um item pelo seu ID
        public Item? GetItemById(string id) => _items.FirstOrDefault(i => i.Id == id);

      
        // Adiciona um novo item ao inventário, com validações
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

        // Atualiza um item existente no inventário
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

        // Remove um item do inventário com base no ID
        public void DeleteItem(string id)
        {
            var item = GetItemById(id);
            if (item != null)
            {
                _items.Remove(item);
                SaveInventory();
            }
        }

        // Devolve todos os itens de uma determinada categoria
        public List<Item> GetItemsByCategory(string category) =>
            _items.Where(i => i.Category == category).ToList();

        // Devolve os itens com quantidade inferior ao limite definido
        public List<Item> GetLowStockItems(int threshold) =>
            _items.Where(i => i.Quantity < threshold).ToList();
    }
}
