using System;

namespace GestaoInventario.Models
{
    /// <summary>
    /// Representa um item no inventário da aplicação.
    /// Contém informações essenciais como nome, quantidade, preço e categoria.
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Identificador único do item.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Nome do item.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Descrição do item.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Quantidade disponível em stock.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Preço do item.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Categoria do item (ex: Informática, Escritório).
        /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// Data e hora da última modificação deste item.
        /// </summary>
        public DateTime LastUpdated { get; set; }
    }
}
