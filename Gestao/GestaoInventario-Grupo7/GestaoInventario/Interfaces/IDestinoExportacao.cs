namespace GestaoInventario.Interfaces
{
    // Representa um destino abstrato para exportação de dados (ex: ficheiro)
    public interface IDestinoExportacao
    {
        // Devolve o caminho onde os dados devem ser exportados
        string? ObterCaminho();
    }
}
