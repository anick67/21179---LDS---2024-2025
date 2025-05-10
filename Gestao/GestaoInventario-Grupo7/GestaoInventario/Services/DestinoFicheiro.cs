using GestaoInventario.Interfaces;
using System.Windows.Forms;

namespace GestaoInventario.Services
{
    public class DestinoFicheiro : IDestinoExportacao
    {
        private readonly string _nomeSugestao;

        public DestinoFicheiro(string nomeSugestao)
        {
            _nomeSugestao = nomeSugestao;
        }

        public string? ObterCaminho()
        {
            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Ficheiros Excel (*.xlsx)|*.xlsx";
                saveDialog.FileName = _nomeSugestao;

                if (saveDialog.ShowDialog() != DialogResult.OK)
                {
                    // Cancelado: devolve null ou string vazia
                    return null;
                }

                return saveDialog.FileName;
            }
        }
    }
}
