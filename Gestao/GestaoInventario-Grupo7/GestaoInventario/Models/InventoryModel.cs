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
        private List<string> _categorias = new List<string>();
        private readonly string _dataFilePath;
        private readonly string _categoriasFilePath;

        // Evento acionado quando um item é adicionado ao inventário
        public event EventHandler<Item>? ItemAdded;

        // Evento acionado quando um item existente é atualizado
        public event EventHandler<Item>? ItemUpdated;

        // Construtor que define o caminho do ficheiro e carrega o inventário
        public InventoryModel(string dataFilePath)
        {
            _dataFilePath = dataFilePath;
            _categoriasFilePath = Path.Combine(Path.GetDirectoryName(dataFilePath)!, "categorias.json");

            LoadInventory();
            LoadCategorias();
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

        // Carrega as categorias do ficheiro separado
        public void LoadCategorias()
        {
            if (File.Exists(_categoriasFilePath))
            {
                try
                {
                    string json = File.ReadAllText(_categoriasFilePath);
                    _categorias = JsonConvert.DeserializeObject<List<string>>(json) ?? new List<string>();
                }
                catch (JsonException)
                {
                    Console.WriteLine("Erro ao carregar categorias: ficheiro pode estar corrompido.");
                    _categorias = new List<string>();
                }
            }
        }

        // Guarda os dados do inventário no ficheiro JSON
        public void SaveInventory()
        {
            string json = JsonConvert.SerializeObject(_items, Formatting.Indented);
            File.WriteAllText(_dataFilePath, json);

            SaveCategorias();
        }

        // Guarda as categorias únicas num ficheiro separado
        public void SaveCategorias()
        {
            var todasCategorias = _items.Select(i => i.Category)
                                        .Where(c => !string.IsNullOrWhiteSpace(c))
                                        .Concat(_categorias)
                                        .Distinct()
                                        .OrderBy(c => c)
                                        .ToList();

            string json = JsonConvert.SerializeObject(todasCategorias, Formatting.Indented);
            File.WriteAllText(_categoriasFilePath, json);
        }

        // Devolve todos os itens do inventário
        public List<Item> GetAllItems() => _items;

        // Devolve todas as categorias únicas, combinando categorias do ficheiro e dos produtos
        public List<string> GetAllCategories()
        {
            return _categorias
                .Concat(_items.Select(i => i.Category!).Where(c => !string.IsNullOrWhiteSpace(c)))
                .Distinct()
                .OrderBy(c => c)
                .ToList();
        }

        // Adiciona uma nova categoria
        public void AdicionarCategoria(string categoria)
        {
            if (!string.IsNullOrWhiteSpace(categoria) && !_categorias.Contains(categoria))
            {
                _categorias.Add(categoria);
                SaveCategorias();
            }
        }

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

        // Atualiza um item com base em parâmetros diretos
        public void UpdateItem(string id, string name, string description, int quantity, decimal price, string category)
        {
            var existing = GetItemById(id);
            if (existing == null) return;

            var itemAtualizado = new Item
            {
                Id = id,
                Name = name,
                Description = description,
                Quantity = quantity,
                Price = price,
                Category = category,
                LastUpdated = DateTime.Now
            };

            UpdateItem(itemAtualizado); // reaproveita o outro método
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