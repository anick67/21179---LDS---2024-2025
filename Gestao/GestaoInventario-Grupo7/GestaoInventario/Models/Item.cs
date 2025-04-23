using System;

namespace GestaoInventario.Models
{
    // Representa um item no inventário da aplicação
    // Contém informações essenciais como nome, quantidade, preço e categoria
    public class Item
    {
      
        // Identificador único do item
        public string? Id { get; set; }

        // Nome do item
        public string? Name { get; set; }

        // Descrição do item
        public string? Description { get; set; }

        // Quantidade disponível em stock
        public int Quantity { get; set; }

        // Preço do item
        public decimal Price { get; set; }
      
        // Categoria do item (ex: Papelaria, Jardim)
        public string? Category { get; set; }

        // Data e hora da última modificação deste item
        public DateTime LastUpdated { get; set; }
    }
}
