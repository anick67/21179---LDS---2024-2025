using GestaoInventario.Interfaces;
using System.Windows.Forms;

namespace GestaoInventario.Services
{
    // Implementa um destino de exportação baseado em ficheiro (via SaveFileDialog)
    public class DestinoFicheiro : IDestinoExportacao
    {
        private readonly string _nomeSugestao;

        // Recebe um nome de ficheiro sugerido para a exportação
        public DestinoFicheiro(string nomeSugestao)
        {
            _nomeSugestao = nomeSugestao;
        }

        // Abre uma caixa de diálogo para escolher o local onde guardar o ficheiro exportado
        public string? ObterCaminho()
        {
            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Ficheiros Excel (*.xlsx)|*.xlsx";
                saveDialog.FileName = _nomeSugestao;

                if (saveDialog.ShowDialog() != DialogResult.OK)
                {
                    // Utilizador cancelou
                    return null;
                }

                return saveDialog.FileName;
            }
        }
    }
}