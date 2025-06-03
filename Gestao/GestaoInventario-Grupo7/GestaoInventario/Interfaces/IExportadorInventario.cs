using System.Collections.Generic;
using GestaoInventario.Models;

namespace GestaoInventario.Interfaces
{
    // Define o contrato para exportar uma coleção de itens do inventário
    // para um destino de exportação (ex: ficheiro, base de dados, etc.)
    public interface IExportadorInventario
    {
        // Exporta os itens fornecidos para o destino especificado
        void Exportar(IEnumerable<Item> items, IDestinoExportacao destino);
    }
}