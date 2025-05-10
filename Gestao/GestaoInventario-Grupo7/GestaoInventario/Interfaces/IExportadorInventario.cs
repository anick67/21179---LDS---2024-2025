using System.Collections.Generic;
using GestaoInventario.Models;

namespace GestaoInventario.Interfaces
{
    public interface IExportadorInventario
    {
        void Exportar(IEnumerable<Item> items, IDestinoExportacao destino);
    }
}
